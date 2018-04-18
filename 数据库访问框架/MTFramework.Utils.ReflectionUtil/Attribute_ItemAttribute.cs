using System;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 定义属性 Item
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Attribute_ItemAttribute : Attribute
    {
        private string memberPropertyName;
        /// <summary>
        /// 构造方法
        /// </summary>
        public Attribute_ItemAttribute()
        {
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="memberPropertyName">成员属性名称</param>
        public Attribute_ItemAttribute(string memberPropertyName)
        {
            this.memberPropertyName = memberPropertyName;
        }
        /// <summary>
        /// 成员属性名称
        /// </summary>
        public string MemberPropertyName
        {
            get
            {
                return this.memberPropertyName;
            }
            set
            {
                this.memberPropertyName = value;
            }
        }
    }
}
