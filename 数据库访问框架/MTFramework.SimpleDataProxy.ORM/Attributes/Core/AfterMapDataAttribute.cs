using System;

namespace MTFramework.SimpleDataProxy.ORM.Attributes
{
    /// <summary>
    /// 填充数据标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterMapDataAttribute : Attribute
    {
        private string tableName;

        /// <summary>
        /// 实例化填充数据标记
        /// </summary>
        public AfterMapDataAttribute()
        {
            this.tableName = string.Empty;
        }

        /// <summary>
        /// 实例化填充数据标记
        /// </summary>
        /// <param name="tableName">表名</param>
        public AfterMapDataAttribute(string tableName)
        {
            this.tableName = string.Empty;
            this.tableName = tableName;
        }

        /// <summary>
        /// 表名
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
