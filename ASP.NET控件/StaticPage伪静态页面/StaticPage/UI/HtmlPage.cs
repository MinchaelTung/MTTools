using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.IO.Compression;
using System.IO;

namespace Eagle.Web.UI {
  public class HtmlPage : Page {
    public override void ProcessRequest(HttpContext context) {
      HttpRequest req = context.Request;
      HttpResponse resp = context.Response;

      HtmlProviderConfigSection section = (HtmlProviderConfigSection)ConfigurationManager.GetSection("eagle/htmlProvider");
      string htmlResource = UrlMapping.AspxToHtml(req.RawUrl);

      if (section.HtmlProvider.Exists(htmlResource)) {
        resp.Redirect(htmlResource);
        return;
      }

      string acceptEncoding = req.Headers.Get("Accept-Encoding").ToLower();

      Stream respStream = resp.Filter;
      FilterStream filterStream = new FilterStream(respStream, htmlResource);
      if (acceptEncoding != null && section.Compress) {
        if (acceptEncoding.Contains("gzip")) {
          resp.AppendHeader("Content-Encoding", "gzip");
          GZipStream gzipStream = new GZipStream(filterStream, CompressionMode.Compress);
          resp.Filter = gzipStream;
        }
      } else {
        resp.Filter = filterStream;
      }

      base.ProcessRequest(context);

    }

    protected override void SavePageStateToPersistenceMedium(object state) {
      
    }
  }
}
