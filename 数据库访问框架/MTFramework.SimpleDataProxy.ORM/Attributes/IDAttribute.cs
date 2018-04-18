using System;
using System.Data;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 资料主键标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class IDAttribute : FieldAttribute
    {
        private bool _isOut;

        /// <summary>
        /// 资料主键标记
        /// </summary>
        /// <param name="columnName">列名称</param>
        public IDAttribute(string columnName)
            : base(columnName)
        {
        }

        /// <summary>
        /// 资料主键标记
        /// </summary>
        /// <param name="columnName">列名称</param>
        /// <param name="dataType">数据类型</param>
        public IDAttribute(string columnName, DbType dataType)
            : base(columnName, dataType)
        {
        }

        /// <summary>
        /// 资料主键标记
        /// </summary>
        /// <param name="columnName">列名称</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="_isOut">是否输出</param>
        public IDAttribute(string columnName, DbType dataType, bool _isOut)
            : base(columnName, dataType)
        {
            this._isOut = _isOut;
        }
    }
}
