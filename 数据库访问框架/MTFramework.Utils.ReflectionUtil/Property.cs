using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    ///  属性
    /// </summary>
    public class Property : IMember
    {
        private DynamicMemberHandle dynamicMemberHandle;
        private PropertyInfo propertyInfo;

        /// <summary>
        /// Property
        /// </summary>
        /// <param name="propertyInfo"></param>
        public Property(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentException("参数:'propertyInfo'不能为 Null");
            }
            this.propertyInfo = propertyInfo;
            this.dynamicMemberHandle = new DynamicMemberHandle(propertyInfo);
        }

        /// <summary>
        /// GetValue
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        object IMember.GetValue(object target)
        {
            object obj2 = null;
            try
            {
                obj2 = this.dynamicMemberHandle.DynamicMemberGet(target);
            }
            catch (Exception exception)
            {
                throw new ReflectionException(string.Format("Type:{0}读取Property:{1}错误", this.ReflectedType, this.propertyInfo.Name), exception);
            }
            return obj2;
        }

        /// <summary>
        /// SetValue
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        void IMember.SetValue(object target, object value)
        {
            try
            {
                object obj2 = value;
                if (((value != null) && (this.dynamicMemberHandle.MemberType != value.GetType())) && ReflectionUtilities.IsImplementFromInterface(this.dynamicMemberHandle.MemberType, typeof(IConvertible)))
                {
                    obj2 = Convert.ChangeType(value, this.dynamicMemberHandle.MemberType);
                }
                this.dynamicMemberHandle.DynamicMemberSet(target, obj2);
            }
            catch (Exception exception)
            {
                throw new ReflectionException(string.Format("Type:{0}设置Property:{1}错误", this.ReflectedType, this.propertyInfo.Name), exception);
            }
        }

        /// <summary>
        /// GetCustomAttributes
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public object[] GetCustomAttributes(System.Type attributeType, bool inherit)
        {
            return this.propertyInfo.GetCustomAttributes(attributeType, inherit);
        }

        /// <summary>
        /// CanRead
        /// </summary>
        public bool CanRead
        {
            get
            {
                return this.propertyInfo.CanRead;
            }
        }

        /// <summary>
        /// CanWrite
        /// </summary>
        public bool CanWrite
        {
            get
            {
                return this.propertyInfo.CanWrite;
            }
        }

        /// <summary>
        /// MemberType
        /// </summary>
        public MemberType MemberType
        {
            get
            {
                return MemberType.Property;
            }
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                return this.propertyInfo.Name;
            }
        }

        /// <summary>
        /// ReflectedType
        /// </summary>
        public System.Type ReflectedType
        {
            get
            {
                return this.propertyInfo.ReflectedType;
            }
        }

        /// <summary>
        /// Type
        /// </summary>
        public System.Type Type
        {
            get
            {
                return this.propertyInfo.PropertyType;
            }
        }
    }

}
