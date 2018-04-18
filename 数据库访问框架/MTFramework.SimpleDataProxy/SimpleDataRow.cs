using System;
using System.Collections.Generic;
using System.Text;

namespace MTFramework.SimpleDataProxy
{
    /// <summary>
    /// SimpleDataTable 的资料行信息
    /// </summary>
    public class SimpleDataRow
    {
        private const string DataColumnNotFound = "列名\"{0}\"不存在?";
        private const string DataColumnIsNotSpecifiedType = "\"{0}\"列类型不是{1}?";
        private const string DataRowTypeConfilcting = "输入的变量\"{0}\"的类型是\"{1}\",无法转换为\"{2}\"";

        private SimpleDataTable _Parent;
        private List<SimpleDataRow.RowValue> _list;

        internal SimpleDataRow(SimpleDataTable parent)
        {
            this._list = new List<SimpleDataRow.RowValue>();
            this._Parent = parent;
            using (IEnumerator<SimpleDataColumn> enumerator = this._Parent.Columns.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    SimpleDataRow.RowValue value = new SimpleDataRow.RowValue();
                    value.Type = enumerator.Current.Type;
                    this._list.Add(value);
                }
            }
        }

        private bool CheckType(Type columnType, Type objectType)
        {
            if (objectType == columnType)
            {
                return true;
            }
            if (objectType.IsSubclassOf(columnType))
            {
                return true;
            }
            return false;
        }

        private int getOrdinaryIndex(string columnName)
        {
            for (int num1 = 0; num1 < this._Parent.Columns.Count; num1++)
            {
                if (this._Parent.Columns[num1].ColumnName == columnName)
                {
                    return num1;
                }
            }
            throw new SimpleDataProxyException(string.Format(DataColumnNotFound, columnName));
        }

        /// <summary>
        /// 得指定资料列的值做为 bool 
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>指定的资料列值。 </returns>
        public Nullable<bool> GetBoolean(int index)
        {
            SimpleDataRow.RowValue rowValue = this._list[index];
            if (rowValue.Value == DBNull.Value)
            {
                return new Nullable<bool>();
            }
            if (!this.CheckType(rowValue.Type, (Type)typeof(bool)))
            {
                throw new SimpleDataProxyException(string.Format(DataColumnIsNotSpecifiedType, this._Parent.Columns[index].ColumnName, "bool"));
            }
            return new Nullable<bool>(Convert.ToBoolean(rowValue.Value));
        }

        /// <summary>
        /// 得指定资料列的值做为 bool 
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>指定的资料列值。 </returns>
        public Nullable<bool> GetBoolean(string columnName)
        {
            return this.GetBoolean(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 得指定资料列的值做为 byte  
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>指定的资料列值。 </returns>
        public Nullable<byte> GetByte(int index)
        {
            SimpleDataRow.RowValue rowValue = this._list[index];
            if (rowValue.Value == DBNull.Value)
            {
                return new Nullable<byte>();
            }
            if (!this.CheckType(rowValue.Type, (Type)typeof(byte)))
            {
                throw new SimpleDataProxyException(string.Format(DataColumnIsNotSpecifiedType, this._Parent.Columns[index].ColumnName, "byte"));
            }
            return new Nullable<byte>(Convert.ToByte(rowValue.Value));
        }

        /// <summary>
        /// 得指定资料列的值做为 byte  
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>指定的资料列值。 </returns>
        public Nullable<byte> GetByte(string columnName)
        {
            return this.GetByte(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 得指定资料列的值做为 byte数组 
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>指定的资料列值。 </returns>
        public byte[] GetBytes(int index)
        {
            SimpleDataRow.RowValue rowValue = this._list[index];
            if (rowValue.Value == DBNull.Value)
            {
                return null;
            }
            if (!this.CheckType(rowValue.Type, (Type)typeof(byte[])))
            {
                throw new SimpleDataProxyException(string.Format(DataColumnIsNotSpecifiedType, this._Parent.Columns[index].ColumnName, "byte[]"));
            }
            return (byte[])rowValue.Value;
        }

        /// <summary>
        /// 得指定资料列的值做为 byte数组  
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>指定的资料列值。 </returns>
        public byte[] GetBytes(string columnName)
        {
            return this.GetBytes(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 得指定资料列的值做为 DateTime 
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>指定的资料列值。 </returns>
        public Nullable<DateTime> GetDateTime(int index)
        {
            SimpleDataRow.RowValue rowValue = this._list[index];
            if (rowValue.Value == DBNull.Value)
            {
                return new Nullable<DateTime>();
            }
            if (!this.CheckType(rowValue.Type, (Type)typeof(DateTime)))
            {
                throw new SimpleDataProxyException(string.Format(DataColumnIsNotSpecifiedType, this._Parent.Columns[index].ColumnName, "DateTime"));
            }
            return new Nullable<DateTime>(Convert.ToDateTime(rowValue.Value));
        }

        /// <summary>
        /// 得指定资料列的值做为 DateTime 
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>指定的资料列值。 </returns>
        public Nullable<DateTime> GetDateTime(string columnName)
        {
            return this.GetDateTime(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 得指定资料列的值做为 decimal 
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>指定的资料列值。 </returns>
        public Nullable<decimal> GetDecimal(int index)
        {
            SimpleDataRow.RowValue rowValue = this._list[index];
            if (rowValue.Value == DBNull.Value)
            {
                return new Nullable<decimal>();
            }
            if (!this.CheckType(rowValue.Type, (Type)typeof(decimal)))
            {
                throw new SimpleDataProxyException(string.Format(DataColumnIsNotSpecifiedType, this._Parent.Columns[index].ColumnName, "Decimal"));
            }
            return new Nullable<decimal>(Convert.ToDecimal(rowValue.Value));
        }

        /// <summary>
        /// 得指定资料列的值做为 decimal 
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<decimal> GetDecimal(string columnName)
        {
            return this.GetDecimal(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 得指定资料列的值做为 double 
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<double> GetDouble(int index)
        {
            SimpleDataRow.RowValue rowValue = this._list[index];
            if (rowValue.Value == DBNull.Value)
            {
                return new Nullable<double>();
            }
            if (!this.CheckType(rowValue.Type, (Type)typeof(double)))
            {
                throw new SimpleDataProxyException(string.Format(DataColumnIsNotSpecifiedType, this._Parent.Columns[index].ColumnName, "Double"));
            }
            return new Nullable<double>(Convert.ToDouble(rowValue.Value));
        }

        /// <summary>
        /// 得指定资料列的值做为 double 
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<double> GetDouble(string columnName)
        {
            return this.GetDouble(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 得指定资料列的值做为 float 
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<float> GetFloat(int index)
        {
            SimpleDataRow.RowValue rowValue = this._list[index];
            if (rowValue.Value == DBNull.Value)
            {
                return new Nullable<float>();
            }
            if (!this.CheckType(rowValue.Type, (Type)typeof(bool)))
            {
                throw new SimpleDataProxyException(string.Format(DataColumnIsNotSpecifiedType, this._Parent.Columns[index].ColumnName, "Float"));
            }
            return new Nullable<float>(Convert.ToSingle(rowValue.Value));
        }

        /// <summary>
        /// 得指定资料列的值做为 float 
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<float> GetFloat(string columnName)
        {
            return this.GetFloat(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 得指定资料列的值做为 Guid 
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<Guid> GetGuid(int index)
        {
            SimpleDataRow.RowValue rowValue = this._list[index];
            if (rowValue.Value == DBNull.Value)
            {
                return new Nullable<Guid>();
            }
            if (!this.CheckType(rowValue.Type, (Type)typeof(Guid)))
            {
                throw new SimpleDataProxyException(string.Format(DataColumnIsNotSpecifiedType, this._Parent.Columns[index].ColumnName, "Guid"));
            }
            return new Nullable<Guid>((Guid)rowValue.Value);
        }

        /// <summary>
        /// 得指定资料列的值做为 Guid 
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<Guid> GetGuid(string columnName)
        {
            return this.GetGuid(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 得指定资料列的值做为 short 
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<short> GetInt16(int index)
        {
            SimpleDataRow.RowValue rowValue = this._list[index];
            if (rowValue.Value == DBNull.Value)
            {
                return new Nullable<short>();
            }
            if (!this.CheckType(rowValue.Type, (Type)typeof(short)))
            {
                throw new SimpleDataProxyException(string.Format(DataColumnIsNotSpecifiedType, this._Parent.Columns[index].ColumnName, "Int16"));
            }
            return new Nullable<short>(Convert.ToInt16(rowValue.Value));
        }

        /// <summary>
        /// 得指定资料列的值做为 short 
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<short> GetInt16(string columnName)
        {
            return this.GetInt16(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 得指定资料列的值做为 int 
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<int> GetInt32(int index)
        {
            SimpleDataRow.RowValue rowValue = this._list[index];
            if (rowValue.Value == DBNull.Value)
            {
                return new Nullable<int>();
            }
            if (!this.CheckType(rowValue.Type, (Type)typeof(int)))
            {
                throw new SimpleDataProxyException(string.Format(DataColumnIsNotSpecifiedType, this._Parent.Columns[index].ColumnName, "Int32"));
            }
            return new Nullable<int>(Convert.ToInt32(rowValue.Value));
        }

        /// <summary>
        /// 得指定资料列的值做为 int 
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<int> GetInt32(string columnName)
        {
            return this.GetInt32(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 得指定资料列的值做为 long 
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<long> GetInt64(int index)
        {
            SimpleDataRow.RowValue rowValue = this._list[index];
            if (rowValue.Value == DBNull.Value)
            {
                return new Nullable<long>();
            }
            if (!this.CheckType(rowValue.Type, (Type)typeof(long)))
            {
                throw new SimpleDataProxyException(string.Format(DataColumnIsNotSpecifiedType, this._Parent.Columns[index].ColumnName, "Int64"));
            }
            return new Nullable<long>(Convert.ToInt64(rowValue.Value));
        }

        /// <summary>
        /// 得指定资料列的值做为 long 
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>指定的资料列值。</returns>
        public Nullable<long> GetInt64(string columnName)
        {
            return this.GetInt64(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 得指定资料列的值做为 string 
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>指定的资料列值。</returns>
        public string GetString(int index)
        {
            SimpleDataRow.RowValue rowValue = this._list[index];
            if (rowValue.Value == DBNull.Value)
            {
                return null;
            }
            if (!this.CheckType(rowValue.Type, (Type)typeof(string)))
            {
                throw new SimpleDataProxyException(string.Format(DataColumnIsNotSpecifiedType, this._Parent.Columns[index].ColumnName, "String"));
            }
            return rowValue.Value.ToString();
        }

        /// <summary>
        /// 得指定资料列的值做为 string 
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>指定的资料列值。</returns>
        public string GetString(string columnName)
        {
            return this.GetString(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 检查指定资料列的值是否为 Null 。
        /// </summary>
        /// <param name="index">以零起始的资料列序数。</param>
        /// <returns>检查指定资料列的值是否为Null。</returns>
        public bool IsDBNull(int index)
        {
            if (this._list[index].Value == DBNull.Value)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查指定资料列的值是否为 Null 。
        /// </summary>
        /// <param name="columnName">资料列的名称。</param>
        /// <returns>检查指定资料列的值是否为 Null。</returns>
        public bool IsDBNull(string columnName)
        {
            return this.IsDBNull(this.getOrdinaryIndex(columnName));
        }

        /// <summary>
        /// 列总数
        /// </summary>
        public int ColumnCount
        {
            get
            {
                return _list.Count;
            }
        }

        /// <summary>
        /// 取得或设定储存于指定资料列的资料。
        /// </summary>
        /// <param name="index">下标</param>
        /// <returns>储存于指定资料列的资料。</returns>
        public object this[int index]
        {
            get
            {
                return this._list[index].Value;
            }
            set
            {
                Type valueType = this._list[index].Type;
                if (value != null)
                {
                    if (!this.CheckType(valueType, value.GetType()))
                    {
                        throw new SimpleDataProxyException(string.Format(DataRowTypeConfilcting, value, value.GetType(), valueType));
                    }
                    this._list[index].Value = value;
                }
                else
                {
                    this._list[index].Value = DBNull.Value;
                }
            }
        }

        /// <summary>
        /// 取得或设定储存于指定资料列的资料。
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <returns>储存于指定资料列的资料。</returns>
        public object this[string columnName]
        {
            get
            {
                return this[this.getOrdinaryIndex(columnName)];
            }
            set
            {
                this[this.getOrdinaryIndex(columnName)] = value;
            }
        }



        internal SimpleDataTable Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        private class RowValue
        {
            public RowValue()
            {
            }

            private Type _Type;
            /// <summary>
            /// 获取资料行的类型
            /// </summary>
            public Type Type
            {
                get { return _Type; }
                set { _Type = value; }
            }
            private object _Value;
            /// <summary>
            /// 获取资料行的值
            /// </summary>
            public object Value
            {
                get { return _Value; }
                set { _Value = value; }
            }
        }
    }
}
