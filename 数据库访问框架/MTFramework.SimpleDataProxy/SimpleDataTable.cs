using System;
using System.Collections.Generic;
using System.Text;

namespace MTFramework.SimpleDataProxy
{
    /// <summary>
    /// SimpleDataTable 简单的数据表
    /// </summary>
    public class SimpleDataTable
    {
        private SimpleDataRowCollection _Rows;
        private SimpleDataColumnCollection _Columns;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SimpleDataTable()
        {
            this._Rows = new SimpleDataRowCollection(this);
            this._Columns = new SimpleDataColumnCollection();
        }

        /// <summary>
        /// 创建一个 属于该 SimpleDataTable 的 SimpleDataRow
        /// </summary>
        /// <returns>返回新的行对象</returns>
        public SimpleDataRow CreateRow()
        {
            return new SimpleDataRow(this);
        }

        /// <summary>
        /// 数据行集合 
        /// </summary>
        public SimpleDataRowCollection Rows
        {
            get { return _Rows; }
            set { _Rows = value; }
        }

        /// <summary>
        /// 数据列头集合 
        /// </summary>
        public SimpleDataColumnCollection Columns
        {
            get { return _Columns; }
            set { _Columns = value; }
        }
    }

}
