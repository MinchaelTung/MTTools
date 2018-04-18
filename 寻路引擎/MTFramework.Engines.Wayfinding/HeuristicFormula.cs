
namespace MTFramework.Engines.MapPathfindingEngine
{
    /// <summary>
    /// 算法类型
    /// </summary>
    public enum HeuristicFormula
    {
        /// <summary>
        /// 传统算法
        /// </summary>
        Custom = 6,
        /// <summary>
        /// 对角捷径算法
        /// </summary>
        DiagonalShortCut = 3,
        /// <summary>
        /// 欧氏算法
        /// </summary>
        Euclidean = 4,
        /// <summary>
        /// 欧氏算法不含SQR算法
        /// </summary>
        EuclideanNotSQR = 5,
        /// <summary>
        /// 曼哈斯顿启发式算法
        /// </summary>
        Manhattan = 1,
        /// <summary>
        /// 最大目标距离算法
        /// </summary>
        MaxDXDY = 2
    }
}
