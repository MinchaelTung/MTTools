using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Security;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Threading;
using System.Security.Cryptography.X509Certificates;


namespace WebApiDemoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开始------------");
            //Install-Package Microsoft.AspNet.WebApi.Client

            demo1();
            //demo2(1);
            //demo3();

            //MvcApiClientDemo_Get().Wait();
            //MvcApiClientDemo_Get(1).Wait();
            //MvcApiClientDemo_Post().Wait();
            //MvcApiClientDemo_PUT(1).Wait();
            //MvcApiClientDemo_Delete(2).Wait();
            //MvcApiClientDemo_Get().Wait();
            Console.WriteLine("结束------------");
            Console.ReadLine();
        }
        #region --- Install-Package Microsoft.AspNet.WebApi.Client Begin ---

        static async Task MvcApiClientDemo_Get()
        {
            Console.WriteLine("----------------------------------------");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:58852/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response = await client.GetAsync("api/userinfo");
                // response.Result
                if (response.IsSuccessStatusCode)
                {
                    List<UserInfoCCCC> product = await response.Content.ReadAsAsync<List<UserInfoCCCC>>();
                    foreach (var item in product)
                    {
                        Console.WriteLine(item.ToString());
                    }

                }
            }
        }

        static async Task MvcApiClientDemo_Get(int id)
        {
            Console.WriteLine("----------------------------------------");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:58852/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/userinfo/" + id);
                // response.Result
                if (response.IsSuccessStatusCode)
                {
                    UserInfoCCCC product = await response.Content.ReadAsAsync<UserInfoCCCC>();
                    Console.WriteLine(product.ToString());



                }
            }
        }
        static async Task MvcApiClientDemo_Post()
        {
            Console.WriteLine("----------------------------------------");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:58852/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var user = new UserInfoCCCC() { ID = 99, Name = "Demo", Book = "Book", Age = 25, Url = "Demo" };
                HttpResponseMessage response = await
                     client.PostAsJsonAsync("api/userinfo", user);

                if (response.IsSuccessStatusCode)
                {
                    //获取发送的URL
                    //Uri url = response.Headers.Location;
                    //Console.WriteLine(url.ToString());

                    UserInfoCCCC u = await response.Content.ReadAsAsync<UserInfoCCCC>();
                    Console.WriteLine(u.ToString());
                    Console.WriteLine("--查询--");
                    MvcApiClientDemo_Get(user.ID).Wait();
                }
            }
        }

        static async Task MvcApiClientDemo_PUT(int id)
        {
            Console.WriteLine("----------------------------------------");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:58852/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/userinfo/" + id);

                if (response.IsSuccessStatusCode)
                {
                    UserInfoCCCC uu = await response.Content.ReadAsAsync<UserInfoCCCC>();

                    uu.Name = "修改了";
                    uu.Book = "修改了";

                    HttpResponseMessage response2 = await client.PutAsJsonAsync("api/userinfo", uu);

                    if (response2.IsSuccessStatusCode)
                    {
                        MvcApiClientDemo_Get(id).Wait();
                    }
                }
            }
        }

        static async Task MvcApiClientDemo_Delete(int id)
        {
            Console.WriteLine("----------------------------------------");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:58852/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync("api/userinfo/" + id);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("删除成功");
                }
            }
        }

        #endregion --- Install-Package Microsoft.AspNet.WebApi.Client End ---

        #region --- Newtonsoft.Json Begin ---
        //using Newtonsoft.Json.Linq;
        //using Newtonsoft.Json;

        static void demo1()
        {
            HttpWebRequest httpWebRequest = WebRequest.Create("http://localhost:58852/h/GetAll") as HttpWebRequest;
            httpWebRequest.KeepAlive = false;
            httpWebRequest.Timeout = 15 * 1000;
            httpWebRequest.Method = "GET";

            string returnStr = string.Empty;
            using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                returnStr = reader.ReadToEnd();
            }
            Console.WriteLine(returnStr);
            //不定对象类型
            //   JArray jArray = (JArray)JsonConvert.DeserializeObject(returnStr);
            //  JObject o = (JObject)jArray[1]; 

            List<UserInfoCCCC> list = JsonConvert.DeserializeObject<List<UserInfoCCCC>>(returnStr);

            foreach (var item in list)
            {
                Console.WriteLine(item.ToString());
            }




        }


        static void demo2(int id)
        {
            HttpWebRequest httpWebRequest = WebRequest.Create("http://localhost:58852/h/get?id=" + id) as HttpWebRequest;
            httpWebRequest.KeepAlive = false;
            httpWebRequest.Timeout = 15 * 1000;
            httpWebRequest.Method = "GET";

            string returnStr = string.Empty;
            using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                returnStr = reader.ReadToEnd();
            }

            JObject jObject = (JObject)JsonConvert.DeserializeObject(returnStr);

            UserInfoCCCC cc = JsonConvert.DeserializeObject<UserInfoCCCC>(returnStr);
            Console.WriteLine(cc.ToString());


        }


        static void demo3()
        {
            UserInfoCCCC uu = new UserInfoCCCC { ID = 99, Name = "DEMO", Book = "DEMO", Age = 25, Url = "DEMO" };


            HttpWebRequest httpWebRequest = WebRequest.Create("http://localhost:58852/h/add") as HttpWebRequest;
            httpWebRequest.KeepAlive = false;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Timeout = 15 * 1000;
            httpWebRequest.Method = "POST";

            byte[] data = Encoding.UTF8.GetBytes("jsonStr=" + JsonConvert.SerializeObject(uu));

            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            string returnStr = string.Empty;
            using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                returnStr = reader.ReadToEnd();
            }

            JObject jObject = (JObject)JsonConvert.DeserializeObject(returnStr);

            UserInfoCCCC cc = JsonConvert.DeserializeObject<UserInfoCCCC>(returnStr);
            Console.WriteLine(cc.ToString());




        }

        #endregion --- Newtonsoft.Json End ---


    }

    public class HttpWebResponseUtility
    {

        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        //string loginUrl = "https://passport.baidu.com/?login";  
        //string userName = "userName";  
        //string password = "password";  
        //string tagUrl = "http://cang.baidu.com/"+userName+"/tags";  
        //Encoding encoding = Encoding.GetEncoding("gb2312");  

        //IDictionary<string, string> parameters = new Dictionary<string, string>();  
        //parameters.Add("tpl", "fa");  
        //parameters.Add("tpl_reg", "fa");  
        //parameters.Add("u", tagUrl);  
        //parameters.Add("psp_tt", "0");  
        //parameters.Add("username", userName);  
        //parameters.Add("password", password);  
        //parameters.Add("mem_pass", "1");  
        //HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(loginUrl, parameters, null, null, encoding, null);  
        //string cookieString = response.Headers["Set-Cookie"]; 


        //GET


        //string userName = "userName";  
        //string tagUrl = "http://cang.baidu.com/"+userName+"/tags";  
        //CookieCollection cookies = new CookieCollection();//如何从response.Headers["Set-Cookie"];中获取并设置CookieCollection的代码略  
        //response = HttpWebResponseUtility.CreateGetHttpResponse(tagUrl, null, null, cookies);  

        //POST

        //string loginUrl = "http://home.51cto.com/index.php?s=/Index/doLogin";  
        //string userName = "userName";  
        //string password = "password";  

        //IDictionary<string, string> parameters = new Dictionary<string, string>();  
        //parameters.Add("email", userName);  
        //parameters.Add("passwd", password);  

        //HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(loginUrl, parameters, null, null, Encoding.UTF8, null);  

    }
}
