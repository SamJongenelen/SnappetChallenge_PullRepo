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
           name: "Progression",
           url: "{controller}/{action}/",
           defaults: new { controller = "Progression", action = "Index" }
             );

            routes.MapRoute(
                name: "Students",
                url: "{controller}/{action}/",
                defaults: new { controller = "Students" }
             );

            routes.MapMvcAttributeRoutes();
        }
    }
}
