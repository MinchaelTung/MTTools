using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MTFramework.Reflections.DynamicImplementInterface
{
    /// <summary>
    /// 动态运行时类
    /// </summary>
    /// <typeparam name="Interface">接口</typeparam>
    public class DynamicRuntimeClass<Interface>
    {
        private Dictionary<string, Delegate> _implements = new Dictionary<string, Delegate>();
        private Type _implementObjectType;
        private CSharpCodeProviderUtil _csharpCodeProviderUtil = new CSharpCodeProviderUtil();
        private static Dictionary<Type, CompilerResults> _caches = new Dictionary<Type, CompilerResults>();

        /// <summary>
        /// 获取接口类型
        /// </summary>
        public Type InterfaceType
        {
            get;
            protected set;
        }
        /// <summary>
        /// 获取字段池
        /// </summary>
        public Dictionary<string, object> Fields
        {
            get;
            protected set;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <exception cref="System.Exception">Interface必须是接口</exception>
        public DynamicRuntimeClass()
        {
            this.InterfaceType = typeof(Interface);
            if (!this.InterfaceType.IsInterface)
            {
                throw new Exception(this.InterfaceType.FullName + " 必须是接口");
            }
            this.Fields = new Dictionary<string, object>();
        }
        /// <summary>
        /// 实现属性
        /// </summary>
        /// <typeparam name="PropertyType">属性返回类型</typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="get">get委托</param>
        /// <param name="set">set委托</param>
        /// <returns>RuntimeClass本身的对象</returns>
        public DynamicRuntimeClass<Interface> ImplementProperty<PropertyType>(string propertyName, Func<Interface, PropertyType> get, Action<Interface, PropertyType> set)
        {
            if (get != null)
            {
                string key = "get" + propertyName;
                if (_implements.ContainsKey(key))
                    _implements[key] = get;
                else
                    _implements.Add(key, get);
            }

            if (set != null)
            {
                string key = "set" + propertyName;
                if (_implements.ContainsKey(key))
                    _implements[key] = set;
                else
                    _implements.Add(key, set);
            }

            return this;
        }
        /// <summary>
        /// 实现方法
        /// </summary>
        /// <param name="methodName">方法名</param>
        /// <param name="method">委托</param>
        /// <returns>RuntimeClass本身的对象</returns>
        public DynamicRuntimeClass<Interface> ImplementMethod(string methodName, Delegate method)
        {
            if (method != null)
            {
                StringBuilder key = new StringBuilder();
                key.Append(methodName);
                key.Append("(");
                MethodInfo mi = method.Method;
                ParameterInfo[] paramInfos = mi.GetParameters();
                foreach (ParameterInfo p in paramInfos)
                {
                    key.Append(p.ParameterType.FullName);
                    key.Append(",");
                }
                if (key[key.Length - 1] == ',')
                    key[key.Length - 1] = ')';
                else
                    key.Append(")");
                _implements.Add(key.ToString(), method);
            }

            return this;
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <exception cref="System.Exception">代码编译错误或未能获取实现对象的类型</exception>
        /// <returns>实例对象</returns>
        public Interface CreateInstance()
        {
            if (_implementObjectType == null)
            {
                CompilerResults compilerResults = null;

                if (_caches.ContainsKey(this.InterfaceType))
                {
                    compilerResults = _caches[this.InterfaceType];
                }
                else
                {
                    string[] referencedAssemblies;
                    string code = this.GetImplementClassCode(out referencedAssemblies);
                    _csharpCodeProviderUtil.ReferencedAssemblies.AddRange(referencedAssemblies);
                    compilerResults = _csharpCodeProviderUtil.Compile(code);
                    if (compilerResults.Errors.HasErrors)
                        throw new Exception("代码编译错误：" + _csharpCodeProviderUtil.GetCompilerErrorsDetail(compilerResults.Errors));

                    _caches.Add(this.InterfaceType, compilerResults);
                }

                Assembly assembly = compilerResults.CompiledAssembly;
                _implementObjectType = assembly.GetType(typeof(Interface).Name + "Implement");
                if (_implementObjectType == null)
                    throw new Exception("未能获取实现对象的类型");
            }

            return (Interface)Activator.CreateInstance(_implementObjectType, _implements);
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="this">对象</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="args">参数</param>
        public void OnEventHandler(Interface @this, string eventName, params object[] args)
        {
            @this.GetType().GetMethod("On" + eventName, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(@this, args);
        }

        /// <summary>
        /// 获取实现类的代码
        /// <exception cref="System.Exception">占不支持带out、ref的参数</exception>
        /// </summary>
        /// <returns>实现类的代码</returns>
        protected string GetImplementClassCode(out string[] referencedAssemblies)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            List<string> referencedAssemblieList = new List<string>();
            Type type = typeof(Interface);
            StringBuilder code = new StringBuilder();
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Text;");
            code.AppendLine("using System.Reflection;");
            code.AppendLine();
            if (!referencedAssemblieList.Contains(type.Assembly.Location))
                referencedAssemblieList.Add(type.Assembly.Location);
            code.AppendLine(string.Format("public class {0}Implement : {1}", type.Name, type.FullName));
            code.AppendLine("{");
            code.AppendLine("private Dictionary<string, Delegate> _implements;");// 字段

            #region 构造函数

            code.AppendLine(string.Format(@"public {0}Implement(Dictionary<string, Delegate> implements)
{{
this._implements = implements;
}}", type.Name));

            #endregion

            #region 构建属性

            foreach (PropertyInfo p in type.GetProperties(bindingFlags))
            {
                if (!referencedAssemblieList.Contains(p.PropertyType.Assembly.Location))
                    referencedAssemblieList.Add(p.PropertyType.Assembly.Location);
                code.AppendLine(string.Format("public {0} {1}", p.PropertyType.FullName, p.Name));
                code.AppendLine("{");
                if (p.CanRead)
                {
                    code.AppendLine(string.Format(@"get
{{
string key = ""get{0}"";
if (!this._implements.ContainsKey(key))
    throw new NotImplementedException(""属性{0}的get未实现"");
return ({1})this._implements[key].DynamicInvoke(this);
}}", p.Name, p.PropertyType.FullName));
                }
                if (p.CanWrite)
                {
                    code.AppendLine(string.Format(@"set
{{
string key = ""set{0}"";
if (!this._implements.ContainsKey(key))
    throw new NotImplementedException(""属性{0}的set未实现"");
this._implements[key].DynamicInvoke(this, value);
}}", p.Name));
                }
                code.AppendLine("}");
            }

            #endregion

            #region 构建方法

            foreach (MethodInfo m in type.GetMethods(bindingFlags))
            {
                if (m.Name.StartsWith("get_") ||
                    m.Name.StartsWith("set_") ||
                    m.Name.StartsWith("add_") ||
                    m.Name.StartsWith("remove_"))
                    continue;

                if (!referencedAssemblieList.Contains(m.ReturnType.Assembly.Location))
                    referencedAssemblieList.Add(m.ReturnType.Assembly.Location);
                // 形参，如System.Int32 p1,System.String p2
                StringBuilder @paramList = new StringBuilder();
                // 参数类型，如,System.Int32,System.String
                StringBuilder @paramTypes = new StringBuilder();
                // 参数名，如,p1,p2
                StringBuilder @paramNames = new StringBuilder();
                @paramTypes.Append(',');
                @paramNames.Append(',');
                ParameterInfo[] parameterInfos = m.GetParameters();
                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    ParameterInfo param = parameterInfos[i];
                    if (param.IsOut)
                        throw new Exception("占不支持带out的参数");

                    if (param.ParameterType.FullName.EndsWith("&"))
                        throw new Exception("占不支持带ref的参数");

                    @paramList.Append(param.ParameterType.FullName.Replace("&", "")).Append(" p").Append(i).Append(',');
                    @paramTypes.Append(param.ParameterType.FullName.Replace("&", "")).Append(',');

                    @paramNames.Append('p').Append(i).Append(',');

                    if (!referencedAssemblieList.Contains(param.ParameterType.Assembly.Location))
                        referencedAssemblieList.Add(param.ParameterType.Assembly.Location);
                }

                if (m.ReturnType == typeof(void))
                {

                    code.AppendLine(string.Format(@"public {0} {1}({3})
{{
string key = ""{1}({2}{4})"";
if (!this._implements.ContainsKey(key))
    throw new NotImplementedException(""方法{1}未实现"");
this._implements[key].DynamicInvoke(this{5});
}}", "void",
                      m.Name,
                      type.FullName,
                      @paramList.Length > 1 ? @paramList.ToString().TrimEnd(',') : string.Empty,
                      @paramTypes.Length > 1 ? @paramTypes.ToString().TrimEnd(',') : string.Empty,
                      @paramNames.Length > 1 ? @paramNames.ToString().TrimEnd(',') : string.Empty));
                }
                else
                {
                    code.AppendLine(string.Format(@"public {0} {1}({3})
{{
string key = ""{1}({2}{4})"";
if (!this._implements.ContainsKey(key))
    throw new NotImplementedException(""方法{1}未实现"");
return ({0})this._implements[key].DynamicInvoke(this{5});
}}", m.ReturnType.FullName,
                      m.Name,
                      type.FullName,
                      @paramList.Length > 1 ? @paramList.ToString().TrimEnd(',') : string.Empty,
                      @paramTypes.Length > 1 ? @paramTypes.ToString().TrimEnd(',') : string.Empty,
                      @paramNames.Length > 1 ? @paramNames.ToString().TrimEnd(',') : string.Empty));
                }
            }

            #endregion

            #region 构建事件

            foreach (EventInfo e in type.GetEvents(bindingFlags))
            {
                // 形参，如System.Int32 p1,System.String p2
                StringBuilder @paramList = new StringBuilder();
                // 参数名，如p1,p2
                StringBuilder @paramNames = new StringBuilder();
                MethodInfo methodInfo = e.EventHandlerType.GetMethod("Invoke");
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                if (!referencedAssemblieList.Contains(e.EventHandlerType.Assembly.Location))
                    referencedAssemblieList.Add(e.EventHandlerType.Assembly.Location);
                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    ParameterInfo param = parameterInfos[i];
                    @paramList.Append(param.ParameterType.FullName).Append(" p").Append(i).Append(',');

                    @paramNames.Append('p').Append(i).Append(',');

                    if (!referencedAssemblieList.Contains(param.ParameterType.Assembly.Location))
                        referencedAssemblieList.Add(param.ParameterType.Assembly.Location);
                }

                code.AppendLine(string.Format(@"public event {0} {1};
protected void On{1}({2})
{{
    if (this.{1} != null)
        this.{1}({3});
}}",
              e.EventHandlerType.FullName,
              e.Name,
              @paramList.ToString().TrimEnd(','),
              @paramNames.ToString().TrimEnd(',')));
            }

            #endregion

            code.AppendLine("}");

            referencedAssemblies = referencedAssemblieList.ToArray();
            return code.ToString();
        }

    }
}
