using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace MTFramework.SimpleDataProxy
{
    /// <summary>
    /// 存储过程参数资料
    /// </summary>
    public class Parameter
    {
        #region --- Ctors Begin ---
        /// <summary>
        /// 构造函数
        /// </summary>
        public Parameter()
        {
            this._Name = string.Empty;
            this._Direction = ParameterDirection.Input;
            this._Value = DBNull.Value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">参数名称</param>
        public Parameter(string name)
        {
            this._Name = name;
            this._Direction = ParameterDirection.Input;
            this._Value = DBNull.Value;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public Parameter(string name, object value)
        {
            this._Name = name;
            this._Direction = ParameterDirection.Input;

            this._Value = value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="dirtion">参数方向</param>
        public Parameter(string name, ParameterDirection dirtion)
        {
            this._Name = name;
            this._Direction = dirtion;
            this._Value = DBNull.Value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="dirtion">参数方向</param>
        /// <param name="value">参数值</param>
        public Parameter(string name, ParameterDirection dirtion, object value)
        {
            this._Name = name;
            this._Direction = dirtion;
            this._Value = value;
        }

        #endregion --- Ctors End ---

        #region --- Fields Begin ---


        private string _Name = string.Empty;
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }


        private ParameterDirection _Direction = ParameterDirection.Input;
        /// <summary>
        /// 参数方向
        /// </summary>
        public ParameterDirection Direction
        {
            get
            {
                return this._Direction;
            }
            set
            {
                this._Direction = value;
            }
        }

        private object _Value = null;
        /// <summary>
        /// 参数值
        /// </summary>
        public object Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                this._Value = value;
            }
        }

        #endregion --- Fields End ---

        #region --- Events Begin ---
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public delegate void ValueChangedHandler(Parameter parameter);
        /// <summary>
        /// 
        /// </summary>
        private event Parameter.ValueChangedHandler _ValueChanged;

        /// <summary>
        /// 参数变动事件
        /// </summary>
        public event Parameter.ValueChangedHandler ValueChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this._ValueChanged = (Parameter.ValueChangedHandler)Delegate.Combine(this._ValueChanged, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this._ValueChanged = (Parameter.ValueChangedHandler)Delegate.Remove(this._ValueChanged, value);
            }
        }

        #endregion --- Events End ---

        #region --- Functions Begin ---

        /// <summary>
        /// 是否输出
        /// </summary>
        /// <returns></returns>
        public bool IsOut()
        {
            return (this._Direction & ParameterDirection.Output) == ParameterDirection.Output;
        }

        #endregion --- Functions End ---

    }
}
