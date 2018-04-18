using System.Collections.Generic;
using System.Drawing;

namespace MTFramework.Engines.MapPathfindingEngine
{
    /// <summary>
    /// 寻路接口
    /// </summary>
    public interface IPathFinder
    {
        /// <summary>
        /// 寻找路径
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <returns>路径</returns>
        List<PathFinderNode> FindPath(Point start, Point end);
        /// <summary>
        /// 停止寻路
        /// </summary>
        void FindPathStop();
        /// <summary>
        /// 对角线
        /// </summary>
        bool Diagonals { get; set; }
        /// <summary>
        /// 发动公式
        /// </summary>
        HeuristicFormula Formula { get; set; }
        /// <summary>
        /// 发动对角线
        /// </summary>
        bool HeavyDiagonals { get; set; }
        /// <summary>
        /// 发动估计值
        /// </summary>
        int HeuristicEstimate { get; set; }
        /// <summary>
        /// 搜索限制
        /// </summary>
        int SearchLimit { get; set; }
        /// <summary>
        /// 停止后
        /// </summary>
        bool Stopped { get; }
        /// <summary>
        /// 决定
        /// </summary>
        bool TieBreaker { get; set; }
    }
}
