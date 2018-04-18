using System;
using System.Collections.Generic;
using System.Drawing;

namespace MTFramework.Engines.MapPathfindingEngine
{
    /// <summary>
    /// 寻路
    /// </summary>
    public class PathFinder : IPathFinder
    {
        //关闭
        private List<PathFinderNode> _mClose = new List<PathFinderNode>();
        //对角线
        private bool _mDiagonals = true;
        //发动公式
        private HeuristicFormula _mFormula = HeuristicFormula.Manhattan;
        //地图坐标
        private byte[,] _mGrid;
        //发动对角线
        private bool _mHeavyDiagonals;
        //发动估计值
        private int _mHEstimate = 2;
        //开放
        private PriorityQueue<PathFinderNode> _mOpen = new PriorityQueue<PathFinderNode>(new ComparePFNode());
        //搜索限制
        private int _mSearchLimit = 2000;
        //停止
        private bool _mStop;
        //停止后
        private bool _mStopped = true;
        //决定
        private bool _mTieBreaker;

        public PathFinder(byte[,] grid)
        {
            if (grid == null)
            {
                throw new ArgumentNullException("参数: grid 不能为Null");
            }
            this._mGrid = grid;
        }
        /// <summary>
        /// 寻找路径
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <returns>路径</returns>
        public List<PathFinderNode> FindPath(Point start, Point end)
        {
            PathFinderNode node;
            sbyte[,] numArray;
            bool flag = false;
            int upperBound = this._mGrid.GetUpperBound(0);
            int num2 = this._mGrid.GetUpperBound(1);
            this._mStop = false;
            this._mStopped = false;
            this._mOpen.Clear();
            this._mClose.Clear();
            if (this._mDiagonals == true)
            {
                numArray = new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
            }
            else
            {
                numArray = new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
            }
            node.G = 0;
            node.H = this._mHEstimate;
            node.F = node.G + node.H;
            node.X = start.X;
            node.Y = start.Y;
            node.PX = node.X;
            node.PY = node.Y;
            this._mOpen.Push(node);
            while ((this._mOpen.Count > 0) && !this._mStop)
            {
                node = this._mOpen.Pop();
                if ((node.X == end.X) && (node.Y == end.Y))
                {
                    this._mClose.Add(node);
                    flag = true;
                    break;
                }
                if (this._mClose.Count > this._mSearchLimit)
                {
                    this._mStopped = true;
                    return null;
                }
                for (int i = 0; i < (this._mDiagonals ? 8 : 4); i++)
                {
                    PathFinderNode node2;
                    node2.X = node.X + numArray[i, 0];
                    node2.Y = node.Y + numArray[i, 1];
                    if (((node2.X >= 0) && (node2.Y >= 0)) && ((node2.X < upperBound) && (node2.Y < num2)))
                    {
                        int num4;
                        if (this._mHeavyDiagonals && (i > 3))
                        {
                            num4 = node.G + ((int)(this._mGrid[node2.X, node2.Y] * 2.41));
                        }
                        else
                        {
                            num4 = node.G + this._mGrid[node2.X, node2.Y];
                        }
                        if (num4 != node.G)
                        {
                            int num5 = -1;
                            for (int j = 0; j < this._mOpen.Count; j++)
                            {
                                if ((this._mOpen[j].X == node2.X) && (this._mOpen[j].Y == node2.Y))
                                {
                                    num5 = j;
                                    break;
                                }
                            }
                            if ((num5 == -1) || (this._mOpen[num5].G > num4))
                            {
                                int num7 = -1;
                                for (int k = 0; k < this._mClose.Count; k++)
                                {
                                    if ((this._mClose[k].X == node2.X) && (this._mClose[k].Y == node2.Y))
                                    {
                                        num7 = k;
                                        break;
                                    }
                                }
                                if ((num7 == -1) || (this._mClose[num7].G > num4))
                                {
                                    node2.PX = node.X;
                                    node2.PY = node.Y;
                                    node2.G = num4;
                                    switch (this._mFormula)
                                    {
                                        case HeuristicFormula.MaxDXDY:
                                            node2.H = this._mHEstimate * Math.Max(Math.Abs((int)(node2.X - end.X)), Math.Abs((int)(node2.Y - end.Y)));
                                            break;

                                        case HeuristicFormula.DiagonalShortCut:
                                            {
                                                int num9 = Math.Min(Math.Abs((int)(node2.X - end.X)), Math.Abs((int)(node2.Y - end.Y)));
                                                int num10 = Math.Abs((int)(node2.X - end.X)) + Math.Abs((int)(node2.Y - end.Y));
                                                node2.H = ((this._mHEstimate * 2) * num9) + (this._mHEstimate * (num10 - (2 * num9)));
                                                break;
                                            }
                                        case HeuristicFormula.Euclidean:
                                            node2.H = (int)(this._mHEstimate * Math.Sqrt(Math.Pow((double)(node2.X - end.X), 2.0) + Math.Pow((double)(node2.Y - end.Y), 2.0)));
                                            break;

                                        case HeuristicFormula.EuclideanNotSQR:
                                            node2.H = (int)(this._mHEstimate * (Math.Pow((double)(node2.X - end.X), 2.0) + Math.Pow((double)(node2.Y - end.Y), 2.0)));
                                            break;

                                        case HeuristicFormula.Custom:
                                            {
                                                Point point = new Point(Math.Abs((int)(end.X - node2.X)), Math.Abs((int)(end.Y - node2.Y)));
                                                int num11 = Math.Abs((int)(point.X - point.Y));
                                                int num12 = Math.Abs((int)(((point.X + point.Y) - num11) / 2));
                                                node2.H = this._mHEstimate * (((num12 + num11) + point.X) + point.Y);
                                                break;
                                            }
                                        default:
                                            node2.H = this._mHEstimate * (Math.Abs((int)(node2.X - end.X)) + Math.Abs((int)(node2.Y - end.Y)));
                                            break;
                                    }
                                    if (this._mTieBreaker)
                                    {
                                        int num13 = node.X - end.X;
                                        int num14 = node.Y - end.Y;
                                        int num15 = start.X - end.X;
                                        int num16 = start.Y - end.Y;
                                        int num17 = Math.Abs((int)((num13 * num16) - (num15 * num14)));
                                        node2.H += (int)(num17 * 0.001);
                                    }
                                    node2.F = node2.G + node2.H;
                                    this._mOpen.Push(node2);
                                }
                            }
                        }
                    }
                }
                this._mClose.Add(node);
            }
            if (flag)
            {
                PathFinderNode node3 = this._mClose[this._mClose.Count - 1];
                for (int m = this._mClose.Count - 1; m >= 0; m--)
                {
                    if (((node3.PX == this._mClose[m].X) && (node3.PY == this._mClose[m].Y)) || (m == (this._mClose.Count - 1)))
                    {
                        node3 = this._mClose[m];
                    }
                    else
                    {
                        this._mClose.RemoveAt(m);
                    }
                }
                this._mStopped = true;
                return this._mClose;
            }
            this._mStopped = true;
            return null;
        }
        /// <summary>
        /// 停止寻路
        /// </summary>
        public void FindPathStop()
        {
            this._mStop = true;
        }

        //[DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        //public static extern unsafe bool ZeroMemory(byte* destination, int length);
        /// <summary>
        /// 对角线
        /// </summary>
        public bool Diagonals
        {
            get
            {
                return this._mDiagonals;
            }
            set
            {
                this._mDiagonals = value;
            }
        }
        /// <summary>
        /// 发动公式
        /// </summary>
        public HeuristicFormula Formula
        {
            get
            {
                return this._mFormula;
            }
            set
            {
                this._mFormula = value;
            }
        }
        /// <summary>
        /// 发动对角线
        /// </summary>
        public bool HeavyDiagonals
        {
            get
            {
                return this._mHeavyDiagonals;
            }
            set
            {
                this._mHeavyDiagonals = value;
            }
        }
        /// <summary>
        /// 发动估计值
        /// </summary>
        public int HeuristicEstimate
        {
            get
            {
                return this._mHEstimate;
            }
            set
            {
                this._mHEstimate = value;
            }
        }
        /// <summary>
        /// 搜索限制
        /// </summary>
        public int SearchLimit
        {
            get
            {
                return this._mSearchLimit;
            }
            set
            {
                this._mSearchLimit = value;
            }
        }
        /// <summary>
        /// 停止后
        /// </summary>
        public bool Stopped
        {
            get
            {
                return this._mStopped;
            }
        }
        /// <summary>
        /// 决定
        /// </summary>
        public bool TieBreaker
        {
            get
            {
                return this._mTieBreaker;
            }
            set
            {
                this._mTieBreaker = value;
            }
        }



        internal class ComparePFNode : IComparer<PathFinderNode>
        {
            public int Compare(PathFinderNode x, PathFinderNode y)
            {
                if (x.F > y.F)
                {
                    return 1;
                }
                if (x.F < y.F)
                {
                    return -1;
                }
                return 0;
            }
        }
    }
}
