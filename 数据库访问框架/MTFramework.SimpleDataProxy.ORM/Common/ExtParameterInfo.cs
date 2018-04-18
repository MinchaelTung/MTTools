using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 参数信息
    /// </summary>
    [Attribute_Class(typeof(ExtParameterAttribute), typeof(FieldInfo))]
    public class ExtParameterInfo : FieldInfo
    {
        private string _accessName = string.Empty;
        private bool _isOut;

        /// <summary>
        /// 数据名称
        /// </summary>
        public string AccessName
        {
            get
            {
                return this._accessName;
            }
            set
            {
                this._accessName = value;
            }
        }

        /// <summary>
        /// 是否输出
        /// </summary>
        public bool IsOut
        {
            get
            {
                return this._isOut;
            }
            set
            {
                this._isOut = value;
            }
        }
    }

}
