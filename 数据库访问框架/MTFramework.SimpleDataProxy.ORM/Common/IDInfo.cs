using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 主键标记
    /// </summary>
    [Attribute_Class(typeof(IDAttribute), typeof(FieldInfo))]
    public class IDInfo : FieldInfo
    {
        private bool _isOut;

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
