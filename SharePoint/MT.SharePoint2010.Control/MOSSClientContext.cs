using System;
using System.Net;
using Microsoft.SharePoint.Client;

namespace MT.SharePoint.Control
{
    /// <summary>
    /// SharePoint客户端实例对象，验证登陆授权权限，填充对象
    /// </summary>
    public class MOSSClientContext
    {
        #region --- 字段和属性 Begin ---

        private static ClientContext _ClientContext = null;

        internal static ClientContext ClientContext
        {
            get
            {
                return _ClientContext;
            }
        }

        /// <summary>
        /// 当前连接网站地址
        /// </summary>
        public static string Url
        {
            get
            {
                return _ClientContext.Url;
            }
        }

        private static User _CurrentUser = null;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public static User CurrentUser
        {
            get
            {
                return _CurrentUser;
            }
        }

        #endregion --- 字段和属性 End ---

        #region --- Public Functions Begin ---

        /// <summary>
        /// 设置SSPClientContext
        /// </summary>
        /// <param name="url">SSP网站</param>
        /// <param name="userName">登陆用户</param>
        /// <param name="userPassword">登录密码</param>
        /// <param name="domain">SSP网站域名</param>
        public static void SetClientContext(string url, string userName, string userPassword, string domain)
        {
            _ClientContext = new ClientContext(url);
            _ClientContext.Credentials = new NetworkCredential(userName, userPassword, domain);

        }

        /// <summary>
        /// 设置在计算机所在域的SSPClientContext
        /// </summary>
        /// <param name="url">SSP网站</param>
        /// <param name="userName">登陆用户</param>
        /// <param name="userPassword">登录密码</param>
        public static void SetClientContext(string url, string userName, string userPassword)
        {
            _ClientContext = new ClientContext(url);
            _ClientContext.Credentials = new NetworkCredential(userName, userPassword);
        }

        /// <summary>
        /// 验证登陆
        /// </summary>
        /// <returns>True 登陆成功并生成当前用户信息 </returns>
        public static bool ValidationLogin()
        {
            bool validation = false;
            try
            {
                Web web = MOSSClientContext.FetchClientObject(_ClientContext.Web);
                _CurrentUser = MOSSClientContext.FetchClientObject(web.CurrentUser);

                validation = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return validation;
        }

        /// <summary>
        /// 填充SharePointClientObjectModel对象
        /// </summary>
        /// <typeparam name="T">ClientObject</typeparam>
        /// <param name="t">SharePointClientObjectModel对象</param>
        /// <param name="retrievals">可选的Linq表达式</param>
        /// <returns></returns>
        public static T FetchClientObject<T>(T t, params System.Linq.Expressions.Expression<Func<T, object>>[] retrievals)
            where T : ClientObject
        {
            _ClientContext.Load(t, retrievals);
            _ClientContext.ExecuteQuery();
            return t;
        }

        public static void ExecuteQuery()
        {
            _ClientContext.ExecuteQuery();
        }


        #endregion --- Public Functions End ---

    }
}
