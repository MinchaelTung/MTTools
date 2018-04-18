using System.Collections.Generic;
using System.Collections;

namespace MTFramework.SimpleDataProxy
{
    /// <summary>
    /// SimpleDataTable 的资料行信息集合
    /// </summary>
    public class SimpleDataRowCollection
    {
        private List<SimpleDataRow> _list;

        private SimpleDataTable _parent;

        internal SimpleDataRowCollection(SimpleDataTable parent)
        {
            this._list = new List<SimpleDataRow>();
            this._parent = parent;
        }

        /// <summary>
        /// 取得指定下标的资料行。
        /// </summary>
        /// <param name="index">下标值</param>
        /// <returns>指定的 SimpleDataRow 。</returns>
        public SimpleDataRow this[int index]
        {
            get
            {
                return this._list[index];
            }
        }

        /// <summary>
        /// 取得此集合中的 SimpleDataRowCollection 物件总数。
        /// </summary>        
        public int Count
        {
            get
            {
                return this._list.Count;
            }
        }

        /// <summary>
        /// 加入 SimpleDataRow 至 SimpleDataRowCollection 。
        /// </summary>
        /// <param name="dr">要加入的 SimpleDataRow 。</param>
        public void Add(SimpleDataRow dr)
        {
            this._list.Add(dr);
            dr.Parent = this._parent;
        }

        /// <summary>
        /// 取得集合的 IEnumerator 。
        /// </summary>
        /// <returns>集合的 IEnumerator 。</returns>
        public IEnumerator GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        /// <summary>
        /// 从集合中移除指定的 SimpleDataRow 。 
        /// </summary>
        /// <param name="dr">要移除的 SimpleDataRow 。</param>
        public void Remove(SimpleDataRow dr)
        {
            this._list.Remove(dr);
        }
    }
}
