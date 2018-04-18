
namespace MTFramework.Engines.MapPathfindingEngine
{
    /// <summary>
    /// 优先列队接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPriorityQueue<T>
    {
        T Peek();
        T Pop();
        int Push(T item);
        void Update(int i);
    }
}
