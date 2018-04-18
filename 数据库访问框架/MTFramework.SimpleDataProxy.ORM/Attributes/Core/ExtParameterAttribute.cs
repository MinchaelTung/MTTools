using System;
using System.Data;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 执行参数标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class ExtParameterAttribute : FieldAttribute
    {
        private string _accessName;
        private bool _isOut;

        /// <summary>
        /// 执行参数标记
        /// </summary>
        public ExtParameterAttribute()
        {
            this._accessName = string.Empty;
        }

        /// <summary>
        /// 执行参数标记
        /// </summary>
        /// <param name="accessName">数据名称</param>
        public ExtParameterAttribute(string accessName)
            : this(accessName, false)
        {
        }

        /// <summary>
        /// 执行参数标记
        /// </summary>
        /// <param name="accessName">数据名称</param>
        /// <param name="type">数据类型</param>
        public ExtParameterAttribute(string accessName, DbType type)
            : this(accessName, type, false)
        {
        }

        /// <summary>
        /// 执行参数标记
        /// </summary>
        /// <param name="accessName">数据名称</param>
        /// <param name="isOut">是否输出</param>
        public ExtParameterAttribute(string accessName, bool isOut)
        {
            this._accessName = string.Empty;
            this._accessName = accessName;
            this._isOut = isOut;
        }

        /// <summary>
        /// 执行参数标记
        /// </summary>
        /// <param name="accessName">数据名称</param>
        /// <param name="parameterName">参数名称</param>
        public ExtParameterAttribute(string accessName, string parameterName)
            : this(accessName, parameterName, false)
        {
        }

        /// <summary>
        /// 执行参数标记
        /// </summary>
        /// <param name="accessName">数据名称</param>
        /// <param name="type">数据类型</param>
        /// <param name="isOut">是否输出</param>
        public ExtParameterAttribute(string accessName, DbType type, bool isOut)
            : base(type)
        {
            this._accessName = string.Empty;
            this._accessName = accessName;
            this._isOut = isOut;
        }

        /// <summary>
        /// 执行参数标记
        /// </summary>
        /// <param name="accessName">数据名称</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="type">数据类型</param>
        public ExtParameterAttribute(string accessName, string parameterName, DbType type)
            : base(parameterName, type)
        {
            this._accessName = string.Empty;
            this._accessName = accessName;
        }

        /// <summary>
        /// 执行参数标记
        /// </summary>
        /// <param name="accessName">数据名称</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="isOut">是否输出</param>
        public ExtParameterAttribute(string accessName, string parameterName, bool isOut)
            : base(parameterName)
        {
            this._accessName = string.Empty;
            this._accessName = accessName;
            this._isOut = isOut;
        }

        /// <summary>
        /// 执行参数标记
        /// </summary>
        /// <param name="accessName">数据名称</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="type">数据类型</param>
        /// <param name="isOut">是否输出</param>
        public ExtParameterAttribute(string accessName, string parameterName, DbType type, bool isOut)
            : base(parameterName, type)
        {
            this._accessName = string.Empty;
            this._accessName = accessName;
            this._isOut = isOut;
        }

        /// <summary>
        /// 数据名称
        /// </summary>
        public string AccessName
        {
            get
            {
                return this._accessName;
            }
        }

        /// <summary>
        /// 是否输出
        /// </summary>
        public bool IsOut
        {
            get
            {
                return this._isOut;
            }
            set
            {
                this._isOut = value;
            }
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamerterName
        {
            get
            {
                return base.ColumnName;
            }
        }
    }
}
