using System.Collections.Generic;
using MTFramework.SimpleDataProxy.ORM.Attributes;

namespace MTFramework.SimpleDataProxy.ORM.Common
{

    /// <summary>
    /// 存储过程标记信息
    /// </summary>
    public class AccessInfo
    {
        private AccessType _Accesstype;
        private List<ExtParameterInfo> _extParameters = new List<ExtParameterInfo>();
        private string _name = string.Empty;
        private string _procedure = string.Empty;

        /// <summary>
        /// 存储过程类型
        /// </summary>
        public AccessType Accesstype
        {
            get
            {
                return this._Accesstype;
            }
            protected set
            {
                this._Accesstype = value;
            }
        }

        /// <summary>
        /// 参数集合
        /// </summary>
        public List<ExtParameterInfo> ExtParameters
        {
            get
            {
                return this._extParameters;
            }
            set
            {
                this._extParameters = value;
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
            set
            {
                this._name = value;
            }
        }

        /// <summary>
        /// 存储过程语句
        /// </summary>
        public string Procedure
        {
            get
            {
                return this._procedure;
            }
            set
            {
                this._procedure = value;
            }
        }
    }
}
