using System;
using System.Reflection;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// DynamicMethodHandle
    /// </summary>
    public class DynamicMethodHandle
    {
        private DynamicMethodDelegate dynamicMethod;
        private Type finalArrayElementType;
        private bool hasFinalArrayParam;
        private string methodName = string.Empty;
        private int methodParamsLength;
        private ParameterInfo[] parameterInfoList;
        /// <summary>
        /// DynamicMethodHandle
        /// </summary>
        /// <param name="info">MethodInfo</param>
        public DynamicMethodHandle(MethodInfo info)
        {
            if (info == null)
            {
                this.dynamicMethod = null;
            }
            else
            {
                this.methodName = info.Name;
                ParameterInfo[] parameters = info.GetParameters();
                int length = parameters.Length;
                if ((length > 0) && (((length == 1) && parameters[0].ParameterType.IsArray) || (parameters[length - 1].GetCustomAttributes(typeof(ParamArrayAttribute), true).Length > 0)))
                {
                    this.hasFinalArrayParam = true;
                    this.methodParamsLength = length;
                    this.finalArrayElementType = parameters[length - 1].ParameterType;
                }
                this.dynamicMethod = DynamicMethodHandlerFactory.CreateMethod(info);
            }
            this.parameterInfoList = info.GetParameters();
        }
        /// <summary>
        /// DynamicMethod
        /// </summary>
        public DynamicMethodDelegate DynamicMethod
        {
            get
            {
                return this.dynamicMethod;
            }
        }
        /// <summary>
        /// FinalArrayElementType
        /// </summary>
        public Type FinalArrayElementType
        {
            get
            {
                return this.finalArrayElementType;
            }
        }
        /// <summary>
        /// HasFinalArrayParam
        /// </summary>
        public bool HasFinalArrayParam
        {
            get
            {
                return this.hasFinalArrayParam;
            }
        }
        /// <summary>
        /// MethodName
        /// </summary>
        public string MethodName
        {
            get
            {
                return this.methodName;
            }
        }
        /// <summary>
        /// MethodParamsLength
        /// </summary>
        public int MethodParamsLength
        {
            get
            {
                return this.methodParamsLength;
            }
        }
        /// <summary>
        /// ParameterInfoList
        /// </summary>
        public ParameterInfo[] ParameterInfoList
        {
            get
            {
                return this.parameterInfoList;
            }
        }
    }
}
