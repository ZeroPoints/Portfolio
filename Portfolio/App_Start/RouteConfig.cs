using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portfolio
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");




            routes.MapRoute(
                name: "Robots.txt",
                url: "robots.txt",
                defaults: new { controller = "Home", action = "Robots" }
            );



            //Fractal Custom Route
            routes.MapRoute(
                name: "DefaultController",
                url: "Fractal/{action}",
                defaults: new { controller = "Fractal", action = "Index", id = UrlParameter.Optional }
            );




            routes.MapRoute(
                name: "DefaultController1",
                url: "{action}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
