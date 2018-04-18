using System;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.CombinedTable
{
    /// <summary>
    /// ColumnInfo
    /// </summary>
    internal class ColumnInfo
    {
        private ColumnType columnType = ColumnType.Field;
        private IMember member;
        private string name = string.Empty;
        private Type objectType;
        /// <summary>
        /// ColumnType
        /// </summary>
        internal ColumnType ColumnType
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
        /// Member
        /// </summary>
        internal IMember Member
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

        internal string Name
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

        internal Type ObjectType
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
