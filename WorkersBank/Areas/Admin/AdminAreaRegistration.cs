using System.Web.Mvc;

namespace WorkersBank.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Jobs",
                "Admin/Jobs/{action}/{slug}",
                new { controller = "Jobs", action = "Index", slug = UrlParameter.Optional },
                new[] { "WorkersBank.Areas.Admin.Controllers" });

            context.MapRoute(
                "Customers",
                "Admin/Customers/{action}/{slug}",
                new { controller = "Customers", action = "Index", slug = UrlParameter.Optional },
                new[] { "WorkersBank.Areas.Admin.Controllers" });

            context.MapRoute("Dashboard", "Admin/Dashboard/{action}/{slug}", new { controller = "Dashboard", action = "Index", slug = UrlParameter.Optional }, new[] { "WorkersBank.Areas.Admin.Controllers" });

            //context.MapRoute(
            //    "Admin_default",
            //    "Admin/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}