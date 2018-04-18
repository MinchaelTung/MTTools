using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO.Compression;
using System.IO;
using System.Configuration;

namespace Eagle.Web {
  public class HtmlHandler : IHttpHandler{
    public const int BUFFER_SIZE = 4096;

    #region IHttpHandler

    public bool IsReusable {
      get { return true; }
    }

    public void ProcessRequest(HttpContext context) {
      HttpRequest req = context.Request;
      HttpResponse resp = context.Response;

      HtmlProviderConfigSection section = (HtmlProviderConfigSection)ConfigurationManager.GetSection("eagle/htmlProvider");
      string htmlResource = req.RawUrl;

      if (!section.HtmlProvider.Exists(htmlResource)) {
        resp.Redirect(UrlMapping.HtmlToAspx(htmlResource));
        return;
      }

      string acceptEncoding = req.Headers.Get("Accept-Encoding").ToLower();

      if (acceptEncoding != null && section.Compress) {
        if (acceptEncoding.Contains("gzip")) {
          resp.AppendHeader("Content-Encoding", "gzip");
        }
      }

      HtmlProvider provider = null;
      try {
        provider = section.HtmlProvider;
        provider.Open(htmlResource);

        byte[] data = new byte[BUFFER_SIZE];
        int len = 0;

        while ((len = provider.Read(data, 0, BUFFER_SIZE)) > 0) {
          resp.OutputStream.Write(data, 0, len);
        }
      } finally {
        if (provider != null)
          provider.Dispose();
      }
    }

    #endregion
  }
}
