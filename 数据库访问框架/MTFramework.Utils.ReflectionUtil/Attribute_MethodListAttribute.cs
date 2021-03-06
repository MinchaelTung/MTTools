﻿using System;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 自定义属性 方法列表
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Attribute_MethodListAttribute : Attribute
    {
        private string methodInfoPropertyName = string.Empty;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="methodInfoPropertyName">方法信息成员名称</param>
        public Attribute_MethodListAttribute(string methodInfoPropertyName)
        {
            this.methodInfoPropertyName = methodInfoPropertyName;
        }
        /// <summary>
        /// 方法信息成员名称
        /// </summary>
        public string MethodInfoPropertyName
        {
            get
            {
                return this.methodInfoPropertyName;
            }
            set
            {
                this.methodInfoPropertyName = value;
            }
        }
    }
}
