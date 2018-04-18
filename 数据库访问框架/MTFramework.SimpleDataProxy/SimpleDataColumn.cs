using System;

namespace MTFramework.SimpleDataProxy
{
    /// <summary>
    /// SimpleDataTable的列属性
    /// </summary>
    public class SimpleDataColumn
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SimpleDataColumn()
        {

        }

        private Type _Type;

        /// <summary>
        /// 获取或设定 SimpleDataColumn 的类型
        /// </summary>
        public Type Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private string _ColumnName = string.Empty;
        /// <summary>
        /// 获取或设定 SimpleDataColumn 的名称
        /// </summary>
        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="columnName">SimpleDataColumn名称</param>
        /// <param name="type">数据类型</param>
        internal SimpleDataColumn(string columnName, Type type)
        {
            this._ColumnName = columnName;
            this._Type = type;
        }
    }
}
