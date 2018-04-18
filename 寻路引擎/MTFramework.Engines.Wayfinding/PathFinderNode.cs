using System.Runtime.InteropServices;

namespace MTFramework.Engines.MapPathfindingEngine
{
    /// <summary>
    /// 寻路信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PathFinderNode
    {
        public int F;
        public int G;
        public int H;
        public int X;
        public int Y;
        public int PX;
        public int PY;

    }
}
