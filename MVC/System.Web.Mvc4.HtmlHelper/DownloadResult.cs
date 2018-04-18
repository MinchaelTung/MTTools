using System.IO;

namespace System.Web.Mvc
{
    public class DownloadResult : ActionResult
    {
        private string _FileName;
        private MemoryStream _FileStream;

        public DownloadResult(string fileName, Stream stream)
        {
            byte[] buf = new byte[stream.Length];
            stream.Read(buf, 0, buf.Length);
            stream.Seek(0, SeekOrigin.Begin);
            this._FileName = fileName;
            this._FileStream = new MemoryStream(buf);
        }

        public DownloadResult(string fileName, MemoryStream stream)
        {
            this._FileName = fileName;
            this._FileStream = stream;
        }

        public DownloadResult(string fileName, byte[] date)
        {
            this._FileName = fileName;
            this._FileStream = new MemoryStream(date);
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get
            {
                return this._FileName;
            }
        }

        /// <summary>
        /// 文件流数据
        /// </summary>
        public MemoryStream FileStream
        {
            get
            {
                return this._FileStream;
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpContext httpContext = HttpContext.Current;
            httpContext.Response.Clear();
            httpContext.Response.ContentType = "application/applefile";
            httpContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
            httpContext.Response.Charset = "gb2312";

            httpContext.Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", this._FileName));
            this._FileStream.WriteTo(httpContext.Response.OutputStream);
            this._FileStream.Close();
            httpContext.Response.End();

        }
    }
}