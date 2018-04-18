using System;
using System.Drawing;
using System.Windows.Forms;
using MTFramework.WinForm.Controls.Events;

namespace MTFramework.WinForm.Controls
{
    public partial class LoginForm : Form
    {
        public delegate void LoginValidationEventArgsHandle(object sender, LoginValidationEventArgs e);
        public event LoginValidationEventArgsHandle LoginValidation;
        public LoginForm(string title, Image backgroundImage)
        {
            InitializeComponent();
            //palInput.Width = this.Width;
            //palInput.Height = this.Height;
            this.palInput.Location = new Point(this.Width - this.palInput.Width, this.Height - this.palInput.Height);

            this.lblMsg.Text = string.Empty;
            this.Text = string.IsNullOrEmpty(title) ? "登陆界面" : title;
            this.BackgroundImage = (backgroundImage == null) ? null : new Bitmap(backgroundImage, new Size(this.Width, this.Height));

        }
        #region --- 内部处理事件 Begin ---

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.CloseFunction();
        }


        /// <summary>
        /// 清空事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClean_Click(object sender, EventArgs e)
        {
            this.txtLogin.Text = string.Empty;
            this.txtPassword.Text = string.Empty;
            this.lblMsg.Text = string.Empty;

        }
        /// <summary>
        /// 登陆事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.inputTextValidation())
            {
                if (this.LoginValidation == null)
                {
                    this.lblMsg.Text = "登陆失败请，联系管理员！";
                    return;
                }
                LoginValidationEventArgs args = new LoginValidationEventArgs();
                args.LoginName = this.txtLogin.Text;
                args.LoginPassword = this.txtPassword.Text;
                LoginValidation(this, args);
                switch (args.IsValidation)
                {
                    case LoginStatus.Login:
                        this.CloseFunction();
                        break;
                    case LoginStatus.NameError:
                        this.lblMsg.Text = "登陆帐号错误，请重新输入！";
                        break;
                    case LoginStatus.PasswordError:
                        this.lblMsg.Text = "登陆密码错误，请重新输入！";
                        break;
                    case LoginStatus.Error:
                    default:
                        this.lblMsg.Text = "登陆帐号或密码错误，请重新输入！";
                        break;
                }
            }
        }

        #endregion --- 内部处理事件 End ---

        #region --- 辅助方法 Begin ---

        /// <summary>
        /// 关闭方法
        /// </summary>
        private void CloseFunction()
        {
            this.Close();
            this.Dispose();
        }

        /// <summary>
        /// 检查输入信息
        /// </summary>
        /// <returns></returns>
        private bool inputTextValidation()
        {
            this.lblMsg.Text = string.Empty;

            if (string.IsNullOrEmpty(this.txtLogin.Text) && string.IsNullOrEmpty(this.txtPassword.Text))
            {
                this.lblMsg.Text = "登陆帐号和密码不能为空，请检查！";
                return false;
            }

            if (string.IsNullOrEmpty(this.txtLogin.Text))
            {
                this.lblMsg.Text = "登陆帐号不能为空，请检查！";
                return false;
            }

            if (string.IsNullOrEmpty(this.txtPassword.Text))
            {
                this.lblMsg.Text = "登陆密码不能为空，请检查！";
                return false;
            }

            return true;
        }

        #endregion --- 辅助方法 End ---

        public enum LoginStatus
        {
            Login,
            NameError,
            PasswordError,
            Error
        }
    }

}
