using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWeb
{
    public class ModelHandler : IHttpModule
    {
        public ModelHandler()
        {
        }
        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(this.Application_BeginRequest);
            context.EndRequest += new EventHandler(this.Application_EndRequest);
           
        }
        public void Application_BeginRequest(Object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            HttpContext context = application.Context;
            HttpResponse response = application.Response;
            HttpRequest request = application.Request;

            response.Write("来自Application_BeginRequest");
        }

        public void Application_EndRequest(Object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            HttpContext context = application.Context;
            HttpResponse response = application.Response;
            HttpRequest request = application.Request;

            response.Write("来自Application_EndRequest");
        }
    }
}