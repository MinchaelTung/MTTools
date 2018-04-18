using System;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 存储过程标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class AccessAttribute : Attribute
    {
        private AccessType _Accesstype;
        private string _name;
        private string _procedure;

        /// <summary>
        /// 实例化存储过程标记
        /// </summary>
        /// <param name="type">实行存储过程类型</param>
        /// <param name="procedure">存储过程字符串</param>
        internal AccessAttribute(AccessType type, string procedure)
        {
            this._name = string.Empty;
            this._procedure = string.Empty;
            this._Accesstype = type;
            this._procedure = procedure;
        }

        /// <summary>
        /// 实例化存储过程标记
        /// </summary>
        /// <param name="type">实行存储过程类型</param>
        /// <param name="name">存储过程名称</param>
        /// <param name="procedure">存储过程字符串</param>
        internal AccessAttribute(AccessType type, string name, string procedure)
        {
            this._name = string.Empty;
            this._procedure = string.Empty;
            this._Accesstype = type;
            this._name = name;
            this._procedure = procedure;
        }

        /// <summary>
        /// 实行存储过程类型
        /// </summary>
        public AccessType AccessType
        {
            get
            {
                return this._Accesstype;
            }
        }

        /// <summary>
        /// 存储过程名称
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
        }

        /// <summary>
        /// 存储过程字符串
        /// </summary>
        public string Procedure
        {
            get
            {
                return this._procedure;
            }
        }
    }
}
