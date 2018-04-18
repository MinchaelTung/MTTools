using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace MTFramework.Reflections.DynamicImplementInterface
{
    internal class CSharpCodeProviderUtil
    {
        private CSharpCodeProvider _csharpCodePrivoder;
        private CompilerParameters _compilerParameters;
        //编译缓存记录
        private static Dictionary<int, CompilerResults> _caches;


        /// <summary>
        /// 获取或设置一个值，该值指示是否生成可执行文件。
        /// </summary>
        /// <value>如果应生成可执行文件，则为 true；否则为 false。</value>
        public bool GenerateExecutable
        {
            get
            {
                return _compilerParameters.GenerateExecutable;
            }
            set
            {
                _compilerParameters.GenerateExecutable = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否在内存中生成输出。
        /// </summary>
        /// <value>如果编译器应在内存中生成输出，则为 true；否则为 false。</value>
        public bool GenerateInMemory
        {
            get
            {
                return _compilerParameters.GenerateInMemory;
            }
            set
            {
                _compilerParameters.GenerateInMemory = value;
            }
        }

        /// <summary>
        /// 获取当前项目所引用的程序集。
        /// </summary>
        /// <value>System.Collections.Specialized.StringCollection，包含由源引用以进行编译的程序集名称</value>
        public StringCollection ReferencedAssemblies
        {
            get
            {
                return _compilerParameters.ReferencedAssemblies;
            }
        }

        public CSharpCodeProviderUtil()
        {
            _caches = new Dictionary<int, CompilerResults>();
            _csharpCodePrivoder = new CSharpCodeProvider();
            _compilerParameters = new CompilerParameters();
            _compilerParameters.ReferencedAssemblies.Add("System.dll");
            _compilerParameters.GenerateExecutable = false;
            _compilerParameters.GenerateInMemory = true;
        }

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="source">要编译的源代码</param>
        /// <returns>指示编译结果的 System.CodeDom.Compiler.CompilerResults 对象</returns>
        public CompilerResults Compile(string source)
        {
            CompilerResults compilerResults = null;
            int sourceHastCode = source.GetHashCode();
            if (_caches.ContainsKey(sourceHastCode))
            {
                compilerResults = _caches[sourceHastCode];
            }
            else
            {
                compilerResults = _csharpCodePrivoder.CompileAssemblyFromSource(_compilerParameters, source);
            }
            return compilerResults;
        }

        /// <summary>
        /// 获取编译错误的详细内容
        /// </summary>
        /// <param name="compilerErrors">CompilerErrorCollection对象</param>
        /// <returns></returns>
        public string GetCompilerErrorsDetail(CompilerErrorCollection compilerErrors)
        {
            StringBuilder sb = new StringBuilder();
            foreach (CompilerError ce in compilerErrors)
            {
                sb.AppendLine(string.Format("{0}: {1},行{2},列{3}", ce.IsWarning ? "错误" : "警告", ce.ErrorText, ce.Line, ce.Column));
            }
            return sb.ToString();
        }
    }
}
