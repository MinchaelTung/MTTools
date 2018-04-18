using System;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 子集合标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class Common_ChildListAttribute : Attribute
    {
        private Type itemType;
        private string tableName = string.Empty;

        /// <summary>
        /// 实例化子集合标记
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="itemType">子集合类型</param>
        public Common_ChildListAttribute(string tableName, Type itemType)
        {
            this.tableName = tableName;
            this.itemType = itemType;
        }

        /// <summary>
        /// 子集合类型
        /// </summary>
        public Type ItemType
        {
            get
            {
                return this.itemType;
            }
            set
            {
                this.itemType = value;
            }
        }

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName
        {
            get
            {
                return this.tableName;
            }
            set
            {
                this.tableName = value;
            }
        }
    }

}
