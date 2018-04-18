using System;
using System.Collections.Generic;
using System.Text;
using MTFramework.Utils.ReflectionUtil;
using MTFramework.SimpleDataProxy.ORM.Attributes;

namespace MTFramework.SimpleDataProxy.ORM.Common
{
    /// <summary>
    /// 数据信息
    /// </summary>
    [Attribute_Class(typeof(AfterMapDataAttribute))]
    public class AfterMapDataInfo
    {
        private System.Reflection.MethodInfo methodInfo;
        private string tableName = string.Empty;

        /// <summary>
        /// 数据信息
        /// </summary>
        public System.Reflection.MethodInfo MethodInfo
        {
            get
            {
                return this.methodInfo;
            }
            set
            {
                this.methodInfo = value;
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
