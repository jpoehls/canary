using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Canary.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "", // URL with parameters
                new { controller = "Dashboard", action = "List" } // Parameter defaults
            );

            routes.MapRoute(
                "Squawk", // Route name
                "squawk/{action}", // URL with parameters
                new { controller = "Squawk", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "ClientTest", // Route name
                "ClientTest/{action}", // URL with parameters
                new { controller = "ClientTest", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "Event", // Route name
                "Event/{hash}", // URL with parameters
                new { controller = "Event", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "Dashboard", // Route name
                "dashboard/{action}", // URL with parameters
                new { controller = "Dashboard", action = "List" } // Parameter defaults
            );

            routes.MapRoute(
                "AppDashboard", // Route name
                "{app}/{env}", // URL with parameters
                new { controller = "Dashboard", action = "Index" } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}