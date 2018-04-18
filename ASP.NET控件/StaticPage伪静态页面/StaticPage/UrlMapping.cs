using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Eagle.Web {
  /// <summary>
  /// 该类提供aspx资源与html资源的映射功能。
  /// </summary>
  /// <remarks>
  /// 该给提供两个方法，可以实现aspx资源与html资源见的相互转变。<br/>
  /// aspx资源可以包含请求参数（如：List.aspx?type=s01）。
  /// </remarks>
  public static class UrlMapping {
    public const string PARAM_KEY = "____";
    public const string PARAM_SEPARATER = "___";

    /// <summary>
    /// html资源到aspx资源的转变
    /// </summary>
    /// <param name="resource">html资源路径</param>
    /// <returns>aspx资源路径，可能包含参数</returns>
    public static string HtmlToAspx(string resource) {
      string path = resource.Substring(0, resource.Length - 6);
      path = path.Replace(PARAM_KEY, "?");

      string[] resArr = path.Split('?');
      if (resArr.Length == 1) {
        return String.Format("{0}.aspx", path);
      } else {
        string param = resArr[1].Replace(PARAM_SEPARATER, "&");
        return String.Format("{0}.aspx?{1}", resArr[0], param);
      }
    }

    /// <summary>
    /// aspx资源到html资源的转变
    /// </summary>
    /// <param name="resource">aspx资源路径，可能包含参数</param>
    /// <returns>html资源路径</returns>
    public static string AspxToHtml(string resource) {
      string[] resArr = resource.Split('?');
      if (!resArr[0].EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
        return resource;

      string path = resArr[0].Substring(0, resArr[0].LastIndexOf('.'));

      if (resArr.Length == 1) {
        return String.Format("{0}.xhtml", path);
      } else {
        string param = resArr[1].Replace("&", PARAM_SEPARATER);
        return String.Format("{0}{1}{2}.xhtml", path, PARAM_KEY, param);
      }
    }
  }
}
