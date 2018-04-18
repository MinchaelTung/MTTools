using System.Collections.Generic;

namespace MTFramework.Engines.MapPathfindingEngine
{
    /// <summary>
    /// 优先列队
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T> : IPriorityQueue<T>
    {
        protected List<T> _InnerList;
        protected IComparer<T> _mComparer;

        #region --- ctor Begin ---

        public PriorityQueue()
        {
            this._InnerList = new List<T>();
            this._mComparer = Comparer<T>.Default;
        }

        public PriorityQueue(IComparer<T> comparer)
        {
            this._InnerList = new List<T>();
            this._mComparer = comparer;
        }

        public PriorityQueue(IComparer<T> comparer, int capacity)
        {
            this._InnerList = new List<T>();
            this._mComparer = comparer;
            this._InnerList.Capacity = capacity;
        }

        #endregion --- ctor End ---

        /// <summary>
        /// 清理搜索
        /// </summary>
        public void Clear()
        {
            this._InnerList.Clear();
        }

        protected virtual int OnCompare(int i, int j)
        {
            return this._mComparer.Compare(this._InnerList[i], this._InnerList[j]);
        }

        public T Peek()
        {
            if (this._InnerList.Count > 0)
            {
                return this._InnerList[0];
            }
            return default(T);
        }

        public T Pop()
        {
            T local = this._InnerList[0];
            this._InnerList[0] = this._InnerList[this._InnerList.Count - 1];
            this._InnerList.RemoveAt(this._InnerList.Count - 1);

            int num4;
            int i = 0;

            while (true)
            {
                num4 = i;
                int j = (2 * i) + 1;
                int num3 = (2 * i) + 2;
                if ((this._InnerList.Count > j) && (this.OnCompare(i, j) > 0))
                {
                    i = j;
                }
                if ((this._InnerList.Count > num3) && (this.OnCompare(i, num3) > 0))
                {
                    i = num3;
                }
                if (i != num4)
                {
                    this.SwitchElements(i, num4);
                }
                else
                {
                    break;
                }
            }

            return local;
        }
        public int Push(T item)
        {
            int count = this._InnerList.Count;
            this._InnerList.Add(item);
            while (count != 0)
            {
                int j = (count - 1) / 2;
                if (this.OnCompare(count, j) >= 0)
                {
                    return count;
                }
                this.SwitchElements(count, j);
                count = j;
            }
            return count;
        }
        public void RemoveLocation(T item)
        {
            int index = -1;
            for (int i = 0; i < this._InnerList.Count; i++)
            {
                if (this._mComparer.Compare(this._InnerList[i], item) == 0)
                {
                    index = i;
                }
            }
            if (index != -1)
            {
                this._InnerList.RemoveAt(index);
            }
        }

        protected void SwitchElements(int i, int j)
        {
            T local = this._InnerList[i];
            this._InnerList[i] = this._InnerList[j];
            this._InnerList[j] = local;
        }

        public void Update(int i)
        {
            int num4;
            int num = i;
            while (true)
            {
                if (num == 0)
                {
                    break;
                }
                num4 = (num - 1) / 2;
                if (this.OnCompare(num, num4) >= 0)
                {
                    break;
                }
                this.SwitchElements(num, num4);
                num = num4;
            }
            if (num < i)
            {
                return;
            }
            while (true)
            {
                int j = num;
                int num3 = (2 * num) + 1;
                num4 = (2 * num) + 2;
                if ((this._InnerList.Count > num3) && (this.OnCompare(num, num3) > 0))
                {
                    num = num3;
                }
                if ((this._InnerList.Count > num4) && (this.OnCompare(num, num4) > 0))
                {
                    num = num4;
                }
                if (num == j)
                {
                    return;
                }
                this.SwitchElements(num, j);
            }
        }

        public int Count
        {
            get
            {
                return this._InnerList.Count;
            }
        }

        public T this[int index]
        {
            get
            {
                return this._InnerList[index];
            }
            set
            {
                this._InnerList[index] = value;
                this.Update(index);
            }
        }
    }
}
