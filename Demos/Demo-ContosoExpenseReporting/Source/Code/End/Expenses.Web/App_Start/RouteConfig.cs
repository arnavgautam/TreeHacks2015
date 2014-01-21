namespace Expenses.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            routes.MapRoute(
                name: "Reports",
                url: "reports",
                defaults: new { controller = "Reports", action = "Index" });

            routes.MapRoute(
                name: "NewReport",
                url: "reports/attachreceipt",
                defaults: new { controller = "Reports", action = "AttachReceipt" });

            routes.MapRoute(
                name: "ReportsSummary",
                url: "reports/summary",
                defaults: new { controller = "Reports", action = "Summary" });

            routes.MapRoute(
                name: "AttachReceipt",
                url: "reports/new",
                defaults: new { controller = "Reports", action = "New" });

            routes.MapRoute(
                name: "Report",
                url: "reports/{id}/{action}",
                defaults: new { controller = "Reports", action = "GetOrUpdate" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}