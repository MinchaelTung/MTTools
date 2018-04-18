using System;
using System.Data;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// MultiTables_PropertyDefineInClassAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MultiTables_PropertyDefineInClassAttribute : Attribute
    {
        private string dataFieldName = string.Empty;
        private DbType dataType;
        private string multiTablesName = string.Empty;
        private string propertyName = string.Empty;

        /// <summary>
        /// MultiTables_PropertyDefineInClassAttribute
        /// </summary>
        /// <param name="multiTablesName">资料集合表成员父类属性标记</param>
        /// <param name="dataFieldName">数据字段名</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="dataType">数据类型</param>
        public MultiTables_PropertyDefineInClassAttribute(string multiTablesName, string dataFieldName, string propertyName, DbType dataType)
        {
            this.multiTablesName = multiTablesName;
            this.dataFieldName = dataFieldName;
            this.propertyName = propertyName;
            this.dataType = dataType;
        }

        /// <summary>
        /// 数据字段名
        /// </summary>
        public string DataFieldName
        {
            get
            {
                return this.dataFieldName;
            }
            set
            {
                this.dataFieldName = value;
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
        /// 资料集合表成员父类属性标记
        /// </summary>
        public string MultiTablesName
        {
            get
            {
                return this.multiTablesName;
            }
            set
            {
                this.multiTablesName = value;
            }
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get
            {
                return this.propertyName;
            }
            set
            {
                this.propertyName = value;
            }
        }
    }
}
