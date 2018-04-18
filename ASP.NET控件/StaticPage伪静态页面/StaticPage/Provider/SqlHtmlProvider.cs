using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Configuration;

namespace Eagle.Web {
  public sealed class SqlHtmlProvider : HtmlProvider {
    private DbConnection conn = null;
    private DbDataReader reader = null;

    private HtmlProviderConfigSection section = null;
    private DbProviderFactory factory = null;

    public SqlHtmlProvider() {
      section = (HtmlProviderConfigSection)ConfigurationManager.GetSection("eagle/htmlProvider");
      string connectionStringName = section.ConnectionString;
      if (String.IsNullOrEmpty(connectionStringName))
        throw new ArgumentNullException("必须指定SqlHtmlProvider所需要的连接名称");

      ConnectionStringSettings connSet = ConfigurationManager.ConnectionStrings[connectionStringName];
      factory = DbProviderFactories.GetFactory(connSet.ProviderName);
      conn = factory.CreateConnection();
      conn.ConnectionString = connSet.ConnectionString;
    }

    public override void Write(byte[] buffer, int offset, int count) {
      throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count) {
      throw new NotImplementedException();
    }

    public override void Dispose() {
      if (reader != null) {
        reader.Dispose();
        reader = null;
      }

      if (conn != null) {
        conn.Close();
      }
    }

    public override void Open(string resource) {
      conn.Open();
    }

    public override bool Exists(string resource) {
      try {
        conn.Open();

        return true;
      } finally {
        Dispose();
      }
    }
  }
}
