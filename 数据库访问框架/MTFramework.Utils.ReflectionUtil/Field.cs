using System;
using System.Reflection;

namespace MTFramework.Utils.ReflectionUtil
{
    /// <summary>
    /// 字段内容
    /// </summary>
    public class Field : IMember
    {
        private DynamicMemberHandle dynamicMemberHandle;

        private FieldInfo fieldInfo;
        /// <summary>
        /// 字段构造方法
        /// </summary>
        /// <param name="fieldInfo">属性信息</param>
        public Field(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentException("参数:'fieldInfo'不能为 Null");
            }
            this.fieldInfo = fieldInfo;
            this.dynamicMemberHandle = new DynamicMemberHandle(fieldInfo);
        }
        /// <summary>
        /// GetCustomAttributes
        /// </summary>
        /// <param name="attributeType">attributeType</param>
        /// <param name="inherit">inherit</param>
        /// <returns>object[]</returns>
        public object[] GetCustomAttributes(System.Type attributeType, bool inherit)
        {
            return this.fieldInfo.GetCustomAttributes(attributeType, inherit);
        }
        /// <summary>
        /// GetValue
        /// </summary>
        /// <param name="target">目标</param>
        /// <returns>object</returns>
        public object GetValue(object target)
        {
            object obj2 = null;
            try
            {
                obj2 = this.dynamicMemberHandle.DynamicMemberGet(target);
            }
            catch (Exception exception)
            {
                throw new ReflectionException(string.Format("Type:{0}读取Field:{1}错误", this.ReflectedType, this.fieldInfo.Name), exception);
            }
            return obj2;
        }
        /// <summary>
        /// SetValue
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="value">值</param>
        public void SetValue(object target, object value)
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
                throw new ReflectionException(string.Format("Type:{0}设置Field:{1}错误", this.ReflectedType, this.fieldInfo.Name), exception);
            }
        }
        /// <summary>
        /// CanRead
        /// </summary>
        public bool CanRead
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// CanWrite
        /// </summary>
        public bool CanWrite
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// MemberType
        /// </summary>
        public MemberType MemberType
        {
            get
            {
                return MemberType.Field;
            }
        }
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                return this.fieldInfo.Name;
            }
        }
        /// <summary>
        /// ReflectedType
        /// </summary>
        public System.Type ReflectedType
        {
            get
            {
                return this.fieldInfo.ReflectedType;
            }
        }
        /// <summary>
        /// Type
        /// </summary>
        public System.Type Type
        {
            get
            {
                return this.fieldInfo.FieldType;
            }
        }
    }

}
