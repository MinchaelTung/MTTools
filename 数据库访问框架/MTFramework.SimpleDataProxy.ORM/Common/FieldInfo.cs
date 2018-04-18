using System.Data;
using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 成员信息
    /// </summary>
    [Attribute_Class(typeof(FieldAttribute))]
    public class FieldInfo
    {
        private string columnName = string.Empty;
        private DbType dataType = DbType.Object;
        private IMember member;

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName
        {
            get
            {
                return this.columnName;
            }
            set
            {
                this.columnName = value;
            }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType DataType
        {
            get
            {
                return this.dataType;
            }
            set
            {
                this.dataType = value;
            }
        }

        /// <summary>
        /// 成员信息
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
                if (this.columnName == string.Empty)
                {
                    this.columnName = this.member.Name;
                }
            }
        }
    }
}
