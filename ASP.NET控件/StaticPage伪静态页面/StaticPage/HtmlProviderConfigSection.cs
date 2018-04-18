using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Eagle.Web {
  public class HtmlProviderConfigSection : ConfigurationSection {
    [ConfigurationProperty("connectionStringName",IsRequired=false)]
    public string ConnectionString {
      get { return (string)this["connectionStringName"]; }
      set { this["connectionStringName"] = value; }
    }

    [ConfigurationProperty("type", IsRequired = true)]
    public string TypeString {
      get { return (string)this["type"]; }
      set { this["type"] = value; }
    }

    [ConfigurationProperty("compress",DefaultValue=false,IsRequired=false)]
    public bool Compress {
      get { return (bool)this["compress"]; }
      set { this["compress"] = value; }
    }

    private HtmlProvider _provider = null;
    public HtmlProvider HtmlProvider {
      get {
        if (_provider == null) {
          Type providerType = Type.GetType(TypeString);
          _provider = (HtmlProvider)Activator.CreateInstance(providerType);
          _provider.Compressed = Compress;
        }

        return _provider;
      }
    }
  }
}
