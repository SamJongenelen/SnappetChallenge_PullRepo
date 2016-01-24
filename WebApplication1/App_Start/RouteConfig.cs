using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //default route to students, thats where we wanna be for demo
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Students", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
             name: "Progression",
             url: "{controller}/{action}/",
             defaults: new { controller = "Progression", action = "Index" }
         );

            routes.MapMvcAttributeRoutes();
        }
    }
}
