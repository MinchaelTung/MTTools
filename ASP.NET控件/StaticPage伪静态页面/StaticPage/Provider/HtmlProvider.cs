using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;

namespace Eagle.Web {
  public abstract class HtmlProvider : IDisposable{
    private bool _compressed;
  
    public bool Compressed {
      get {
        return _compressed;
      }
      set {
        _compressed = true;
      }
    }

    public abstract void Write(byte[] buffer, int offset, int count);

    public abstract int Read(byte[] buffer, int offset, int count);

    public abstract bool Exists(string resource);

    public abstract void Open(string resource);

    public abstract void Dispose();
  }
}
