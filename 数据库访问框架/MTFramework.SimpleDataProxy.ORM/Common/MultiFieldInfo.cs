using System.Data;
using MTFramework.SimpleDataProxy.ORM.Attributes;
using MTFramework.Utils.ReflectionUtil;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 集合成员信息
    /// </summary>
    [Attribute_Class(typeof(MultiFieldAttribute))]
    public class MultiFieldInfo
    {
        private DbType _dataType = DbType.Object;
        private string _memberName;
        private string columnName = string.Empty;
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
                return this._dataType;
            }
            set
            {
                this._dataType = value;
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
            }
        }

        /// <summary>
        /// 成员名称
        /// </summary>
        public string MemberName
        {
            get
            {
                return this._memberName;
            }
            set
            {
                this._memberName = value;
            }
        }
    }
}
