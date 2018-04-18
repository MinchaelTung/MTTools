using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace Mvc.Admin
{
    public class AdminAreaRegistration : PortableAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        /// <summary>
        /// 标记当前项目的命名空间
        /// </summary>
        public string AreaControllersNamespace
        {
            get
            {
                return "Mvc.Admin.Controllers";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
        {
            //Resource
            if (context.Routes["ResourceRoute"] == null)
            {
                context.MapRoute(
                    "ResourceRoute",
                    base.AreaRoutePrefix + "/resource/{resourceName}",
                    new { controller = "EmbeddedResource", action = "Index" },
                    new[] { "MvcContrib.PortableAreas" }
                );
            }
            //Scripts
            context.MapRoute(
                AreaName + "_Scripts",
                base.AreaRoutePrefix + "/Scripts/{resourceName}",
                new { controller = "EmbeddedResource", action = "Index", resourcePath = "Scripts" },
                new[] { "MvcContrib.PortableAreas" }
            );
            //Content
            context.MapRoute(
                AreaName + "_Content",
                base.AreaRoutePrefix + "/Content/{resourceName}",
                new { controller = "EmbeddedResource", action = "Index", resourcePath = "Content" },
                new[] { "MvcContrib.PortableAreas" }
            );
            //Default
            context.MapRoute(
                AreaName + "_Default",
                AreaName + "/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { AreaControllersNamespace }
            );
            RegisterAreaEmbeddedResources();
        }
    }
}