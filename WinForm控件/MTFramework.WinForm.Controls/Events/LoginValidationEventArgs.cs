using System;

namespace MTFramework.WinForm.Controls.Events
{
    /// <summary>
    /// 登陆验证事件
    /// </summary>
    public class LoginValidationEventArgs : EventArgs
    {
        public LoginValidationEventArgs()
        {

        }

        private string _LoginName;
        /// <summary>
        /// 登陆帐号名称
        /// </summary>
        public string LoginName
        {
            get
            {
                return this._LoginName;
            }
            internal set
            {
                this._LoginName = value;
            }
        }

        private string _LoginPassword;
        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPassword
        {
            get
            {
                return this._LoginPassword;
            }
            internal set
            {
                this._LoginPassword = value;
            }
        }

        private LoginForm.LoginStatus _IsValidation = LoginForm.LoginStatus.Error;
        /// <summary>
        /// 验证结果
        /// </summary>
        public LoginForm.LoginStatus IsValidation
        {
            get
            {
                return this._IsValidation;
            }
            set
            {
                this._IsValidation = value;
            }
        }


    }
}
