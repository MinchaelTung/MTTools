using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MTFramework.Engines.MapPathfindingEngine
{
    /// <summary>
    /// 快速寻路
    /// </summary>
    public class PathFinderFast : IPathFinder
    {
        private PathFinderNodeFast[] _mCalcGrid;
        private List<PathFinderNode> _mClose = new List<PathFinderNode>();
        private int _mCloseNodeCounter;
        private byte _mCloseNodeValue = 2;
        private bool _mDiagonals = true;
        private sbyte[,] _mDirection = new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
        private int _mEndLocation;
        private HeuristicFormula _mFormula = HeuristicFormula.Manhattan;
        private bool _mFound;
        private byte[,] _mGrid;
        private ushort _mGridX;
        private ushort _mGridXMinus1;
        private ushort _mGridY;
        private ushort _mGridYLog2;
        private int _mH;
        private bool _mHeavyDiagonals;
        private int _mHEstimate = 2;
        private int _mLocation;
        private ushort _mLocationX;
        private ushort _mLocationY;
        private int _mNewG;
        private int _mNewLocation;
        private ushort _mNewLocationX;
        private ushort _mNewLocationY;
        private PriorityQueue<int> _mOpen;
        private byte _mOpenNodeValue = 1;
        private int _mSearchLimit = 0x7d0;
        private bool _mStop;
        private bool _mStopped = true;
        private bool _mTieBreaker;

        public PathFinderFast(byte[,] grid)
        {
            if (grid == null)
            {
                throw new ArgumentNullException("参数：grid 不能为 Null");
            }
            this._mGrid = grid;
            this._mGridX = (ushort)(this._mGrid.GetUpperBound(0) + 1);
            this._mGridY = (ushort)(this._mGrid.GetUpperBound(1) + 1);
            this._mGridXMinus1 = (ushort)(this._mGridX - 1);
            this._mGridYLog2 = (ushort)Math.Log((double)this._mGridY, 2.0);
            if ((Math.Log((double)this._mGridX, 2.0) != ((int)Math.Log((double)this._mGridX, 2.0))) || (Math.Log((double)this._mGridY, 2.0) != ((int)Math.Log((double)this._mGridY, 2.0))))
            {
                throw new ArgumentException("参数:Grid,尺寸错误  X 和 Y 必须是 2 乘方数");
            }
            if ((this._mCalcGrid == null) || (this._mCalcGrid.Length != (this._mGridX * this._mGridY)))
            {
                this._mCalcGrid = new PathFinderNodeFast[this._mGridX * this._mGridY];
            }
            this._mOpen = new PriorityQueue<int>(new ComparePFNodeMatrix(this._mCalcGrid));
        }

        public List<PathFinderNode> FindPath(Point start, Point end)
        {
            lock (this)
            {
                this._mFound = false;
                this._mStop = false;
                this._mStopped = false;
                this._mCloseNodeCounter = 0;
                this._mOpenNodeValue = (byte)(this._mOpenNodeValue + 2);
                this._mCloseNodeValue = (byte)(this._mCloseNodeValue + 2);
                this._mOpen.Clear();
                this._mClose.Clear();
                this._mLocation = (start.Y << this._mGridYLog2) + start.X;
                this._mEndLocation = (end.Y << this._mGridYLog2) + end.X;
                this._mCalcGrid[this._mLocation].G = 0;
                this._mCalcGrid[this._mLocation].F = this._mHEstimate;
                this._mCalcGrid[this._mLocation].PX = (ushort)start.X;
                this._mCalcGrid[this._mLocation].PY = (ushort)start.Y;
                this._mCalcGrid[this._mLocation].Status = this._mOpenNodeValue;
                this._mOpen.Push(this._mLocation);
                while ((this._mOpen.Count > 0) && !this._mStop)
                {
                    this._mLocation = this._mOpen.Pop();
                    if (this._mCalcGrid[this._mLocation].Status != this._mCloseNodeValue)
                    {
                        this._mLocationX = (ushort)(this._mLocation & this._mGridXMinus1);
                        this._mLocationY = (ushort)(this._mLocation >> this._mGridYLog2);
                        if (this._mLocation == this._mEndLocation)
                        {
                            this._mCalcGrid[this._mLocation].Status = this._mCloseNodeValue;
                            this._mFound = true;
                            break;
                        }
                        if (this._mCloseNodeCounter > this._mSearchLimit)
                        {
                            this._mStopped = true;
                            return null;
                        }
                        for (int i = 0; i < (this._mDiagonals ? 8 : 4); i++)
                        {
                            this._mNewLocationX = (ushort)(this._mLocationX + this._mDirection[i, 0]);
                            this._mNewLocationY = (ushort)(this._mLocationY + this._mDirection[i, 1]);
                            this._mNewLocation = (this._mNewLocationY << this._mGridYLog2) + this._mNewLocationX;
                            if (((this._mNewLocationX < this._mGridX) && (this._mNewLocationY < this._mGridY)) && (this._mGrid[this._mNewLocationX, this._mNewLocationY] != 0))
                            {
                                if (this._mHeavyDiagonals && (i > 3))
                                {
                                    this._mNewG = this._mCalcGrid[this._mLocation].G + ((int)(this._mGrid[this._mNewLocationX, this._mNewLocationY] * 2.41));
                                }
                                else
                                {
                                    this._mNewG = this._mCalcGrid[this._mLocation].G + this._mGrid[this._mNewLocationX, this._mNewLocationY];
                                }
                                if (((this._mCalcGrid[this._mNewLocation].Status != this._mOpenNodeValue) && (this._mCalcGrid[this._mNewLocation].Status != this._mCloseNodeValue)) || (this._mCalcGrid[this._mNewLocation].G > this._mNewG))
                                {
                                    this._mCalcGrid[this._mNewLocation].PX = this._mLocationX;
                                    this._mCalcGrid[this._mNewLocation].PY = this._mLocationY;
                                    this._mCalcGrid[this._mNewLocation].G = this._mNewG;
                                    switch (this._mFormula)
                                    {
                                        case HeuristicFormula.MaxDXDY:
                                            this._mH = this._mHEstimate * Math.Max(Math.Abs((int)(this._mNewLocationX - end.X)), Math.Abs((int)(this._mNewLocationY - end.Y)));
                                            break;

                                        case HeuristicFormula.DiagonalShortCut:
                                            {
                                                int num2 = Math.Min(Math.Abs((int)(this._mNewLocationX - end.X)), Math.Abs((int)(this._mNewLocationY - end.Y)));
                                                int num3 = Math.Abs((int)(this._mNewLocationX - end.X)) + Math.Abs((int)(this._mNewLocationY - end.Y));
                                                this._mH = ((this._mHEstimate * 2) * num2) + (this._mHEstimate * (num3 - (2 * num2)));
                                                break;
                                            }
                                        case HeuristicFormula.Euclidean:
                                            this._mH = (int)(this._mHEstimate * Math.Sqrt(Math.Pow((double)(this._mNewLocationY - end.X), 2.0) + Math.Pow((double)(this._mNewLocationY - end.Y), 2.0)));
                                            break;

                                        case HeuristicFormula.EuclideanNotSQR:
                                            this._mH = (int)(this._mHEstimate * (Math.Pow((double)(this._mNewLocationX - end.X), 2.0) + Math.Pow((double)(this._mNewLocationY - end.Y), 2.0)));
                                            break;

                                        case HeuristicFormula.Custom:
                                            {
                                                Point point = new Point(Math.Abs((int)(end.X - this._mNewLocationX)), Math.Abs((int)(end.Y - this._mNewLocationY)));
                                                int num4 = Math.Abs((int)(point.X - point.Y));
                                                int num5 = Math.Abs((int)(((point.X + point.Y) - num4) / 2));
                                                this._mH = this._mHEstimate * (((num5 + num4) + point.X) + point.Y);
                                                break;
                                            }
                                        default:
                                            this._mH = this._mHEstimate * (Math.Abs((int)(this._mNewLocationX - end.X)) + Math.Abs((int)(this._mNewLocationY - end.Y)));
                                            break;
                                    }
                                    if (this._mTieBreaker)
                                    {
                                        int num6 = this._mLocationX - end.X;
                                        int num7 = this._mLocationY - end.Y;
                                        int num8 = start.X - end.X;
                                        int num9 = start.Y - end.Y;
                                        int num10 = Math.Abs((int)((num6 * num9) - (num8 * num7)));
                                        this._mH += (int)(num10 * 0.001);
                                    }
                                    this._mCalcGrid[this._mNewLocation].F = this._mNewG + this._mH;
                                    this._mOpen.Push(this._mNewLocation);
                                    this._mCalcGrid[this._mNewLocation].Status = this._mOpenNodeValue;
                                }
                            }
                        }
                        this._mCloseNodeCounter++;
                        this._mCalcGrid[this._mLocation].Status = this._mCloseNodeValue;
                    }
                }
                if (this._mFound)
                {
                    PathFinderNode node;
                    this._mClose.Clear();
                    int x = end.X;
                    int y = end.Y;
                    PathFinderNodeFast fast = this._mCalcGrid[(end.Y << this._mGridYLog2) + end.X];
                    node.F = fast.F;
                    node.G = fast.G;
                    node.H = 0;
                    node.PX = fast.PX;
                    node.PY = fast.PY;
                    node.X = end.X;
                    node.Y = end.Y;
                    while ((node.X != node.PX) || (node.Y != node.PY))
                    {
                        this._mClose.Add(node);
                        x = node.PX;
                        y = node.PY;
                        fast = this._mCalcGrid[(y << this._mGridYLog2) + x];
                        node.F = fast.F;
                        node.G = fast.G;
                        node.H = 0;
                        node.PX = fast.PX;
                        node.PY = fast.PY;
                        node.X = x;
                        node.Y = y;
                    }
                    this._mClose.Add(node);
                    this._mStopped = true;
                    return this._mClose;
                }
                this._mStopped = true;
                return null;
            }
        }

        public void FindPathStop()
        {
            this._mStop = true;
        }

        //[DllImport("kernel32.DLL", EntryPoint = "RtlZeroMemory")]
        //public static extern unsafe bool ZeroMemory(byte* destination, int length);

        public bool Diagonals
        {
            get
            {
                return this._mDiagonals;
            }
            set
            {
                this._mDiagonals = value;
                if (this._mDiagonals)
                {
                    this._mDirection = new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
                }
                else
                {
                    this._mDirection = new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
                }
            }
        }

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

        public bool Stopped
        {
            get
            {
                return this._mStopped;
            }
        }

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


        internal class ComparePFNodeMatrix : IComparer<int>
        {
            private PathFinderFast.PathFinderNodeFast[] mMatrix;

            public ComparePFNodeMatrix(PathFinderFast.PathFinderNodeFast[] matrix)
            {
                this.mMatrix = matrix;
            }

            public int Compare(int a, int b)
            {
                if (this.mMatrix[a].F > this.mMatrix[b].F)
                {
                    return 1;
                }
                if (this.mMatrix[a].F < this.mMatrix[b].F)
                {
                    return -1;
                }
                return 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct PathFinderNodeFast
        {
            public int F;
            public int G;
            public ushort PX;
            public ushort PY;
            public byte Status;
        }
    }
}
