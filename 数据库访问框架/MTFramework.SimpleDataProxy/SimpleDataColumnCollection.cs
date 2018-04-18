using System;
using System.Collections.Generic;

namespace MTFramework.SimpleDataProxy
{
    /// <summary>
    /// SimpleDataTable的列集合
    /// </summary>
    public class SimpleDataColumnCollection
    {
        private List<SimpleDataColumn> _list;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SimpleDataColumnCollection()
        {
            this._list = new List<SimpleDataColumn>();
        }

        /// <summary>
        /// 取得集合中的总数。
        /// </summary>
        public int Count
        {
            get
            {
                return this._list.Count;
            }
        }

        /// <summary>
        /// 从集合中取得指定下标的 SimpleDataColumn 。  
        /// </summary>
        /// <param name="index">下标</param>
        /// <returns>指定的 SimpleDataColumn 。</returns>
        public SimpleDataColumn this[int index]
        {
            get
            {
                return _list[index];
            }
        }

        /// <summary>
        /// 从集合中取得指定的 SimpleDataColumn 。  
        /// </summary>
        /// <param name="name">下标</param>
        /// <returns>指定的 SimpleDataColumn 。</returns>
        public SimpleDataColumn this[string name]
        {
            get
            {
                foreach (SimpleDataColumn column in _list)
                {
                    if (column.ColumnName == name)
                    {
                        return column;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 建立 SimpleDataColumn 物件，并將它加入 SimpleDataColumnCollection 。
        /// </summary>
        /// <param name="columnName">资料行名称</param>
        /// <param name="type">资料行类型</param>
        public void Add(string columnName, Type type)
        {
            this._list.Add(new SimpleDataColumn(columnName, type));
        }

        /// <summary>
        /// 取得集合的 IEnumerator 。
        /// </summary>
        /// <returns>集合的 IEnumerator 。</returns>
        public IEnumerator<SimpleDataColumn> GetEnumerator()
        {

            return this._list.GetEnumerator();
        }

        /// <summary>
        /// 是否存在指定 Column 。
        /// </summary>
        /// <param name="columnName">Column 名称</param>
        /// <returns>是否存在指定 Column 。</returns>
        public bool HasColumn(string columnName)
        {
            foreach (SimpleDataColumn column in this._list)
            {
                if (column.ColumnName == columnName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
