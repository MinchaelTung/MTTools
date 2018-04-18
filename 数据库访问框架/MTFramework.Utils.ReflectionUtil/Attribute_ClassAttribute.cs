using System;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 自定义属性 Class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class Attribute_ClassAttribute : Attribute
    {
        private Type attributeType;
        private Type parentType;
        private string restrictPropertyName;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="attributeType">类型</param>
        public Attribute_ClassAttribute(Type attributeType)
        {
            this.attributeType = attributeType;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="attributeType">类型</param>
        /// <param name="restrictPropertyName">属性名称</param>
        public Attribute_ClassAttribute(Type attributeType, string restrictPropertyName)
        {
            this.attributeType = attributeType;
            this.restrictPropertyName = restrictPropertyName;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="attributeType">属性类型</param>
        /// <param name="parentType">父类属性</param>
        public Attribute_ClassAttribute(Type attributeType, Type parentType)
        {
            this.attributeType = attributeType;
            this.parentType = parentType;
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="attributeType">属性类型</param>
        /// <param name="parentType">父类属性</param>
        /// <param name="restrictPropertyName">属性名称</param>
        public Attribute_ClassAttribute(Type attributeType, Type parentType, string restrictPropertyName)
        {
            this.attributeType = attributeType;
            this.parentType = parentType;
            this.restrictPropertyName = restrictPropertyName;
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
        /// <summary>
        /// 属性名称
        /// </summary>
        public string RestrictPropertyName
        {
            get
            {
                return this.restrictPropertyName;
            }
            set
            {
                this.restrictPropertyName = value;
            }
        }
    }
}
