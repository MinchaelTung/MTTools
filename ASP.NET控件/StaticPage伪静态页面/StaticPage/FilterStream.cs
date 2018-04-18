using System;
using System.IO;
using System.Configuration;

namespace Eagle.Web {
  public class FilterStream : Stream{
    private Stream respStream = null;
    private HtmlProvider provider = null;

    public FilterStream(Stream responseStream, string resource) {
      respStream = responseStream;
      HtmlProviderConfigSection section = (HtmlProviderConfigSection)ConfigurationManager.GetSection("eagle/htmlProvider");

      try {
        provider = section.HtmlProvider;
        provider.Open(resource);
      } catch {
        if (provider != null) {
          provider.Dispose();
          provider = null;
        }
      }
    }

    #region Implement the Stream
    public override bool CanRead {
      get { return false; }
    }

    public override bool CanSeek {
      get { return false; }
    }

    public override bool CanWrite {
      get { return respStream.CanWrite; }
    }

    public override void Flush() {
      respStream.Flush();
    }

    public override long Length {
      get { return respStream.Length; }
    }

    public override long Position {
      get {
        return respStream.Position;
      }
      set {
        throw new NotImplementedException();
      }
    }

    public override int Read(byte[] buffer, int offset, int count) {
      throw new NotImplementedException();
    }

    public override long Seek(long offset, SeekOrigin origin) {
      throw new NotImplementedException();
    }

    public override void SetLength(long value) {
      throw new NotImplementedException();
    }
    #endregion

    public override void Write(byte[] buffer, int offset, int count) {
      if(provider!=null)
        provider.Write(buffer, offset, count);
      respStream.Write(buffer, offset, count);
    }

    protected override void Dispose(bool disposing) {
      if (provider != null)
        try { provider.Dispose(); } catch { }

      respStream.Dispose();
      base.Dispose(disposing);
    }
  }
}
