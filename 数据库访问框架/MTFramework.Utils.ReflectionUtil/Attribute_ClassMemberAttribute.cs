using System;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 自定义属性 Class 成员
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class Attribute_ClassMemberAttribute : Attribute
    {
        private Type attributeType;
        private Type mapType;
        private string memberName;
        private Type parentType;
        /// <summary>
        /// 构造方法
        /// </summary>
        public Attribute_ClassMemberAttribute()
        {
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="attributeType">属性类型</param>
        /// <param name="parentType">父类属性</param>
        public Attribute_ClassMemberAttribute(Type attributeType, Type parentType)
            : this(attributeType, parentType, "")
        {
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="attributeType">属性类型</param>
        /// <param name="parentType">父类属性</param>
        /// <param name="memberName">属性名称</param>
        public Attribute_ClassMemberAttribute(Type attributeType, Type parentType, string memberName)
            : this(attributeType, parentType, memberName, null)
        {
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="attributeType">属性类型</param>
        /// <param name="parentType">父类属性</param>
        /// <param name="memberName">属性名称</param>
        /// <param name="mapType">属性类型</param>
        public Attribute_ClassMemberAttribute(Type attributeType, Type parentType, string memberName, Type mapType)
        {
            this.attributeType = attributeType;
            this.parentType = parentType;
            this.memberName = memberName;
            this.mapType = mapType;
        }
        /// <summary>
        /// 属性类型
        /// </summary>
        public Type AttributeType
        {
            get
            {
                return this.attributeType;
            }
            set
            {
                this.attributeType = value;
            }
        }
        /// <summary>
        /// 属性类型
        /// </summary>
        public Type MapType
        {
            get
            {
                return this.mapType;
            }
            set
            {
                this.mapType = value;
            }
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string MemberName
        {
            get
            {
                return this.memberName;
            }
            set
            {
                this.memberName = value;
            }
        }
        /// <summary>
        /// 父类属性
        /// </summary>
        public Type ParentType
        {
            get
            {
                return this.parentType;
            }
            set
            {
                this.parentType = value;
            }
        }
    }
}
