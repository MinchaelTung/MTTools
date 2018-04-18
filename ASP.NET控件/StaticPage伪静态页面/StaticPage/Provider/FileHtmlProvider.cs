using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

namespace Eagle.Web {
  public sealed class FileHtmlProvider : HtmlProvider {
    private Stream _stream;

    public override void Write(byte[] buffer, int offset, int count) {
      if (_stream != null)
        _stream.Write(buffer, offset, count);
    }

    public override int Read(byte[] buffer, int offset, int count) {
      if(_stream!=null)
        return _stream.Read(buffer, offset, count);
      return 0;
    }

    public override void Dispose() {
      if (_stream != null) {
        _stream.Dispose();
        _stream = null;
      }
    }

    public override void Open(string resource) {
      string path = HttpContext.Current.Server.MapPath(resource);
      _stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
    }

    public override bool Exists(string resource) {
      string path = HttpContext.Current.Server.MapPath(resource);
      return File.Exists(path);
    }
  }
}
