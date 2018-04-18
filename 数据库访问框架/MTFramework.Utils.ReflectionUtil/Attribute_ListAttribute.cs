using System;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 自定义属性 集合属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Attribute_ListAttribute : Attribute
    {
        private string keyPropertyName;
        private string memberPropertyName;
        /// <summary>
        /// 构造方法
        /// </summary>
        public Attribute_ListAttribute()
        {
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="memberPropertyName">成员属性名称</param>
        public Attribute_ListAttribute(string memberPropertyName)
        {
            this.memberPropertyName = memberPropertyName;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="memberPropertyName">成员属性名称</param>
        /// <param name="keyPropertyName">成员Key名称</param>
        public Attribute_ListAttribute(string memberPropertyName, string keyPropertyName)
        {
            this.memberPropertyName = memberPropertyName;
            this.keyPropertyName = keyPropertyName;
        }
        /// <summary>
        /// 成员Key名称
        /// </summary>
        public string KeyPropertyName
        {
            get
            {
                return this.keyPropertyName;
            }
            set
            {
                this.keyPropertyName = value;
            }
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
