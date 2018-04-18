using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        MessageBox messageBox;

        protected void Page_Load(object sender, EventArgs e)
        {
            messageBox = new MessageBox(this);

            if (IsPostBack == true)
            {
                messageBox.Show("ok", "alert('提交回访');", null);
            }
           
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            messageBox.Show("ok", "测试", "http://www.g.cn", UpdatePanel1, false);

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            messageBox.Show("ok", "测试", null, false);

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            messageBox.Show("ok", "Open", "http://www.baidu.com", UpdatePanel1);

        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            messageBox.Show("ok", "Open", "http://www.baidu.com", null);

        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            messageBox.Show("ok", "alert('就阿斯科利大赛户籍卡山东黄金 ');", null);

        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            messageBox.Show("ok", "alert('就阿斯科利大赛户籍卡山东黄金 ');", UpdatePanel1);

        }
    }
}