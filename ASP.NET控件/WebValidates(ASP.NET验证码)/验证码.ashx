<%@ WebHandler Language="C#" Class="Code" %>

using System;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;

//要在一般处理程序中使用Session 必须 加入 System.Web.SessionState.IRequiresSessionState 接口
public class Code : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    /// <summary>
    /// 设置备选字符
    /// </summary>
    private static string LETTER = "qaGFDzxspQWEwedOPLKcvfrtgbnhAZXyuICVBjmJHSkiolRTYUNM";
    /// <summary>
    /// 设置备选汉字，以剔除一些不雅的汉字
    /// </summary>
    private static string BASESTR ="的一是在不了有和人这中大为上个国我以要他时来用们生到作地于出就分对成会可主发年动同工也能下过子说产种面而方后多定行学法所民得经十三之进着等部度家电力里如水化高自二理起小物现实加量都两体制机当使点从业本去把性好应开它合还因由其些然前外天政四日那社义事平形相全表间样与关各重新线内数正心反你明看原又么利比或但质气第向道命此变条只没结解问意建月公无系军很情者最立代想已通并提直题党程展五果料象员革位入常文总次品式活设及管特件长求老头基资边流路级少图山统接知较将组见计别她手角期根论运农指几九区强放决西被干做必战先回则任取据处队南给色光门即保治北造百规热领七海口东导器压志世金增争济阶油思术极交受联什认六共权收证改清己美再采转更单风切打白教速花带安场身车例真务具万每目至达走积示议声报斗完类八离华名确才科张信马节话米整空元况今集温传土许步群广石记需段研界拉林律叫且究观越织装影算低持音众书布复容儿须际商非验连断深难近矿千周委素技备半办青省列习响约支般史感劳便团往酸历市克何除消构府称太准精值号率族维划选标写存候毛亲快效斯院查江型眼王按格养易置派层片始却专状育厂京识适属圆包火住调满县局照参红细引听该铁价严";
       
    /// <summary>
    /// 设置备选数字和字符
    /// </summary>
    private static string NUMBERANDLETTER = "01UY2345SDFvbnGH6789qwerXZAtyuiplkjhgfdsKLTazPIxcmMNBVCJREWQ";

    private Random rad = new Random();
    public void ProcessRequest (HttpContext context) {
        
        //禁止缓存               
        context.Response.Expires = -9999;
        context.Response.AddHeader("Pragma","no-cache");
        context.Response.AddHeader("cache-ctrol","no-cache");
        context.Response.ContentType = "image/jpeg";
        string str = "";
        for (int i = 0; i < 4; i++)
        {
            str += GetRandomChinese();
        }
        context.Session["Mark"] = str;
        //图片绘制        
        using (Image img = new Bitmap(100, 30)) {
            Graphics grfx = Graphics.FromImage(img);
            grfx.Clear(Color.White);
            for (int i = 0; i < 10; i++) {
                int x1 = rad.Next(img.Width);
                int x2 = rad.Next(img.Width);
                int y1 = rad.Next(img.Height);
                int y2 = rad.Next(img.Height);

                grfx.DrawLine(new Pen(Color.FromArgb(rad.Next(50, 255), rad.Next(50, 255), rad.Next(50, 255))), x1, y1,x2,y2);
            }

            Font font = new Font("Arial", 12, FontStyle.Bold);
           
            for (int i = 0; i < str.Length; i++)
            {
                Color color = Color.FromArgb(rad.Next(0, 120), rad.Next(0, 120), rad.Next(0, 120));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, img.Width, img.Height), color, color, 1.2F, true);
                int x = img.Width / (str.Length + 1)*i + rad.Next(10);
                int y = 2+rad.Next(10);
                grfx.DrawString(str.Substring(i,1), font, brush, x, y);
            }
            grfx.DrawRectangle(new Pen(Brushes.Silver), 0, 0, img.Width - 1, img.Height - 1);
            img.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    
    private string GetRandomNumber() {
        return rad.Next(10).ToString();        
    }
    private string GetRandomLetter()
    {
        int temp = rad.Next(0,LETTER.Length);
        return LETTER.Substring(temp, 1);
    }
    private string GetRandomNumberAndLetter() {
        int temp = rad.Next(0,NUMBERANDLETTER.Length);
        return NUMBERANDLETTER.Substring(temp, 1);
    }
    private string GetRandomChinese() {
        int temp = rad.Next(0,BASESTR.Length);
        return BASESTR.Substring(temp, 1);
    }
}