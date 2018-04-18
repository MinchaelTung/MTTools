using System;
using System.Collections.Generic;

namespace MTFramework.SimpleDataProxy
{
    /// <summary>
    /// 存储过程资料
    /// </summary>   
    public class Procedure
    {
        #region --- Ctors Begin ---
        /// <summary>
        /// 构造函数
        /// </summary>
        public Procedure()
        {
            this._Text = string.Empty;
            this._CommandTimeOut = 0x1e;
            this._Parameters = new List<Parameter>();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text">存储过程语句</param>
        public Procedure(string text)
        {
            this._Text = text;
            this._CommandTimeOut = 0x1e;
            this._Parameters = new List<Parameter>();
        }


        #endregion --- Ctors End ---

        #region --- Fields Begin ---

        private string _Text = string.Empty;
        /// <summary>
        /// 存储工程名称
        /// </summary>
        public string Text
        {
            get
            {
                return this._Text;
            }
            set
            {
                this._Text = value;
            }
        }


        private int _CommandTimeOut = 0x1e;
        /// <summary>
        /// 执行过时
        /// </summary>
        public int CommandTimeOut
        {
            get
            {
                return this._CommandTimeOut;
            }
            set
            {
                this._CommandTimeOut = value;
            }
        }

        private List<Parameter> _Parameters;
        /// <summary>
        /// 参数组
        /// </summary>
        public List<Parameter> Parameters
        {
            get
            {
                return this._Parameters;
            }
            set
            {
                this._Parameters = value;
            }
        }


        #endregion --- Fields End ---

        #region --- Functions Begin ---

        /// <summary>
        /// 删除参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <returns>成功移除返回 True  否则返回 False</returns>
        public bool RemoveParameter(string parameterName)
        {
            foreach (var item in _Parameters)
            {
                if (item.Name == parameterName)
                {
                    _Parameters.Remove(item);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 参数组索引
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <returns>获取或返回参数值</returns>
        public object this[string parameterName]
        {
            set
            {
                foreach (var item in _Parameters)
                {
                    if (item.Name == parameterName)
                    {
                        item.Value = value;
                        return;
                    }
                }
                throw new ArgumentException(string.Format("该 Key:[{0}] 值不存在", parameterName));
            }
            get
            {
                foreach (var item in _Parameters)
                {
                    if (item.Name == parameterName)
                    {
                        return item.Value;
                    }
                }
                throw new ArgumentException(string.Format("该 Key:[{0}] 值不存在", parameterName));
            }
        }

        #endregion --- Functions End ---
    }
}
