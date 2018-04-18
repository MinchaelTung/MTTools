using System;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 列信息
    /// </summary>
    public class ColumnInfo
    {
        private ColumnType columnType = ColumnType.Field;
        private IMember member;
        private string name = string.Empty;
        private Type objectType;

        /// <summary>
        /// 列类型
        /// </summary>
        public ColumnType ColumnType
        {
            get
            {
                return this.columnType;
            }
            set
            {
                this.columnType = value;
            }
        }

        /// <summary>
        /// 列成员
        /// </summary>
        public IMember Member
        {
            get
            {
                return this.member;
            }
            set
            {
                this.member = value;
            }
        }

        /// <summary>
        /// 列名称
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// 值类型
        /// </summary>
        public Type ObjectType
        {
            get
            {
                return this.objectType;
            }
            set
            {
                this.objectType = value;
            }
        }
    }
}
