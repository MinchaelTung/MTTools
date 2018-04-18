
namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 成员内容接口
    /// </summary>
    public interface IMember
    {
        /// <summary>
        /// GetCustomAttributes
        /// </summary>
        /// <param name="attributeType">attributeType</param>
        /// <param name="inherit">inherit</param>
        /// <returns>object[]</returns>
        object[] GetCustomAttributes(System.Type attributeType, bool inherit);
        /// <summary>
        /// GetValue
        /// </summary>
        /// <param name="target">目标</param>
        /// <returns>object</returns>
        object GetValue(object target);
        /// <summary>
        /// SetValue
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="value">值</param>
        void SetValue(object target, object value);
        /// <summary>
        /// CanRead
        /// </summary>
        bool CanRead { get; }
        /// <summary>
        /// CanWrite
        /// </summary>
        bool CanWrite { get; }
        /// <summary>
        /// MemberType
        /// </summary>
        MemberType MemberType { get; }
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// ReflectedType
        /// </summary>
        System.Type ReflectedType { get; }
        /// <summary>
        /// Type
        /// </summary>
        System.Type Type { get; }
    }
}
