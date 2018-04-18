using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace WatermarkImage
{
    public class WImage : Image
    {
        private string WatermarkImagePath
        {
            get{    
                return ConfigurationManager.AppSettings["WatermarkImage"];
            }
        }
        private string WatermarkLogo
        {
            get
            {
                return ConfigurationManager.AppSettings["WatermarkLogo"];
            }
        }
        private static System.Drawing.Image WATERMARKIMAGE = null;

        [CategoryAttribute("Behavior")]
        [Description("图片路径")]
        public override string ImageUrl
        {
            get
            {
                return base.ImageUrl;
            }
            set
            {
                base.ImageUrl = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Img;
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                this.WaterImageUrl(base.ImageUrl);
                base.Render(writer);
            }
            catch (Exception ex) {
                //错误记录
                string filePath = HttpContext.Current.Server.MapPath("~/App_Data/WatermarkImageExection.txt");
                string error = String.Format("{0}\r\n{1}\r\n\r\n************************************\r\n", ex.Message, ex.StackTrace);
                File.AppendAllText(filePath, error, Encoding.Default);
            }
        }

        private void WaterImageUrl(string imageUrl)
        {
            int index = imageUrl.LastIndexOf("/");
            string begin = imageUrl.Substring(0, index);
            string end = imageUrl.Substring(index);
            base.ImageUrl = string.Format("{0}{1}{2}", begin, WatermarkImagePath, end);
            this.CreateWatermarkImage(imageUrl, base.ImageUrl);
        }

        private void CreateWatermarkImage(string sourceUrl, string currUrl)
        {
            if (!File.Exists(Page.Server.MapPath(currUrl)))
            {
                if (WATERMARKIMAGE == null)
                {
                    WATERMARKIMAGE = System.Drawing.Image.FromFile(Page.Server.MapPath(WatermarkLogo));
                }
                using (System.Drawing.Image img = System.Drawing.Image.FromFile(Page.Server.MapPath(sourceUrl)))
                {
                    System.Drawing.Graphics grp = System.Drawing.Graphics.FromImage(img);
                    int x = img.Width - WATERMARKIMAGE.Width;
                    int y = img.Height - WATERMARKIMAGE.Height;
                    grp.DrawImage(WATERMARKIMAGE, x, y, WATERMARKIMAGE.Width, WATERMARKIMAGE.Height);
                    img.Save(Page.Server.MapPath(currUrl), System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }
    }
}
