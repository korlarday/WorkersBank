using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WorkersBank
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Home", "Home/{action}/{slug}", new { controller = "Home", action = "Index", slug = UrlParameter.Optional }, new[] { "WorkersBank.Controllers" });
            routes.MapRoute("Applicants", "Applicants/{action}/{slug}", new { controller = "Applicants", action = "Index", slug = UrlParameter.Optional }, new[] { "WorkersBank.Controllers" });
            routes.MapRoute("Accounts", "Accounts/{action}/{slug}", new { controller = "Accounts", action = "Index", slug = UrlParameter.Optional }, new[] { "WorkersBank.Controllers" });
            routes.MapRoute("Default", "", new { controller = "Home", action = "Index" }, new[] { "WorkersBank.Controllers" });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
