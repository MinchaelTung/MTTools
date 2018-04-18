using System;

namespace MTFramework.OfficeConvertToUtil
{
    /// <summary>
    /// Office 文档转换执行结果信息
    /// </summary>
    public class OfficeConvertResultInfo
    {
        #region --- 属性 Begin ---

        private OfficeConvertResult _Result = OfficeConvertResult.UnknownError;
        /// <summary>
        /// Office 文件转换结果
        /// </summary>
        public OfficeConvertResult Result
        {
            get
            {
                return this._Result;
            }
            set
            {
                this._Result = value;
            }
        }

        private string _ResultMsg = string.Empty;
        /// <summary>
        /// 返回结果信息 文件路径等
        /// </summary>        
        public string ResultMsg
        {
            get
            {
                return this._ResultMsg;
            }
            set
            {
                this._ResultMsg = value;
            }
        }


        private Exception _Exception = null;
        /// <summary>
        /// 错误信息
        /// </summary>
        public Exception Exception
        {
            get
            {
                return this._Exception;
            }
            set
            {
                this._Exception = value;
            }
        }

        #endregion --- 属性 End ---

        #region --- 默认构造函数 Begin ---

        public OfficeConvertResultInfo(OfficeConvertResult result)
        {
            this.Result = result;
        }

        public OfficeConvertResultInfo(OfficeConvertResult result, string resultMsg)
        {
            this.Result = result;
            this.ResultMsg = resultMsg;
        }

        public OfficeConvertResultInfo(OfficeConvertResult result, string resultMsg, Exception exception)
        {
            this.Result = result;
            this.ResultMsg = resultMsg;
            this.Exception = exception;
        }
        #endregion --- 默认构造函数 End ---

    }
}
