using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

namespace Watermark
{
    public class WatermarkImage : Image
    {
        private const string WATERIMAGEPATH = "/watermarkimage";
        private const string WATERMARKIMAGEURL = "~/images/watermarkimage/watermark.png";
        private static System.Drawing.Image WATERMARKIMAGE = null;

        protected override void Render(HtmlTextWriter writer)
        {
            this.WaterImageUrl(base.ImageUrl);
            base.Render(writer);
        }

        private void WaterImageUrl(string imageUrl)
        {
            int index = imageUrl.LastIndexOf("/");
            string begin=imageUrl.Substring(0,index);
            string end = imageUrl.Substring(index);
            base.ImageUrl = string.Format("{0}{1}{2}", begin, WATERIMAGEPATH, end);
            this.CreateWatermarkImage(imageUrl, base.ImageUrl);
        }

        private void CreateWatermarkImage(string sourceUrl, string currUrl)
        {
            if (!File.Exists(Page.Server.MapPath(currUrl))) {
                if (WATERMARKIMAGE == null) {
                    WATERMARKIMAGE = System.Drawing.Image.FromFile(Page.Server.MapPath(WATERMARKIMAGEURL));
                }
                using (System.Drawing.Image img = System.Drawing.Image.FromFile(Page.Server.MapPath(sourceUrl)))
                {
                    
                    
                    System.Drawing.Graphics grp=System.Drawing.Graphics.FromImage(img);
                    
                    int x=img.Width-WATERMARKIMAGE.Width;
                    int y=img.Height-WATERMARKIMAGE.Height;
                    grp.DrawImage(WATERMARKIMAGE,x,y,WATERMARKIMAGE.Width,WATERMARKIMAGE.Height);

                    img.Save(Page.Server.MapPath(currUrl), System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }
    }
}
