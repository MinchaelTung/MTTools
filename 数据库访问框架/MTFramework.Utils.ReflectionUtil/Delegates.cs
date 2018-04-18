
namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 异步核心代理
    /// </summary>
    /// <returns>返回一个结果对象</returns>
    public delegate object DynamicCtorDelegate();

    /// <summary>
    /// 异步成员设置代理
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="value">目标值</param>
    public delegate void DynamicMemberSetDelegate(object target, object value);

    /// <summary>
    /// 异步成员读取代理
    /// </summary>
    /// <param name="target">目标</param>
    /// <returns>返回目标对象</returns>
    public delegate object DynamicMemberGetDelegate(object target);

    /// <summary>
    /// 异步方法代理
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="args">参数</param>
    /// <returns>返回一个结果对象</returns>
    public delegate object DynamicMethodDelegate(object target, object[] args);
}
