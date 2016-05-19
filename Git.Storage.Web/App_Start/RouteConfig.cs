using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Git.Framework.Controller.Mvc;

namespace Git.Storage.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        { 
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.CreateArea("Root", "Git.Storage.Web.Controllers", routes.MapRoute("Root_Default", "{controller}/{action}", new { controller = "Home", action = "Index" }));
        }

        /// <summary>
        /// 注册产品和物料管理路径
        /// </summary>
        /// <param name="routes"></param>
        private static void RegisterProduct(RouteCollection routes)
        {
            routes.CreateArea("Product", "Git.Storage.Web.Areas.Product.Controllers", routes.MapRoute("Product_Default", "{controller}/{action}", new { controller = "Home", action = "Index" }));
        }
    }
}