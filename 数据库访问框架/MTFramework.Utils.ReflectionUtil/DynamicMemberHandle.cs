using System;
using System.Reflection;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// DynamicMemberHandle
    /// </summary>
    public class DynamicMemberHandle
    {
        private DynamicMemberGetDelegate dynamicMemberGet;
        private DynamicMemberSetDelegate dynamicMemberSet;
        private string memberName;
        private Type memberType;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="info">字段信息</param>
        public DynamicMemberHandle(FieldInfo info)
            : this(info.Name, info.FieldType, DynamicMethodHandlerFactory.CreateFieldGetter(info), DynamicMethodHandlerFactory.CreateFieldSetter(info))
        {
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="info">属性信息</param>
        public DynamicMemberHandle(PropertyInfo info)
            : this(info.Name, info.PropertyType, DynamicMethodHandlerFactory.CreatePropertyGetter(info), DynamicMethodHandlerFactory.CreatePropertySetter(info))
        {
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="memberName">memberName</param>
        /// <param name="memberType">memberType</param>
        /// <param name="dynamicMemberGet">dynamicMemberGet</param>
        /// <param name="dynamicMemberSet">dynamicMemberSet</param>
        private DynamicMemberHandle(string memberName, Type memberType, DynamicMemberGetDelegate dynamicMemberGet, DynamicMemberSetDelegate dynamicMemberSet)
        {
            this.memberName = string.Empty;
            this.memberName = memberName;
            this.memberType = memberType;
            this.dynamicMemberGet = dynamicMemberGet;
            this.dynamicMemberSet = dynamicMemberSet;
        }
        /// <summary>
        /// 获取成员读取代理
        /// </summary>
        public DynamicMemberGetDelegate DynamicMemberGet
        {
            get
            {
                return this.dynamicMemberGet;
            }
        }
        /// <summary>
        /// 获取成员设置代理
        /// </summary>
        public DynamicMemberSetDelegate DynamicMemberSet
        {
            get
            {
                return this.dynamicMemberSet;
            }
        }
        /// <summary>
        /// 成员名称
        /// </summary>
        public string MemberName
        {
            get
            {
                return this.memberName;
            }
        }
        /// <summary>
        /// 成员类型
        /// </summary>
        public Type MemberType
        {
            get
            {
                return this.memberType;
            }
        }
    }
}
