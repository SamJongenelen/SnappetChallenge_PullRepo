using System.Web.Http;

namespace WebApplication1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute(
                name: "Progress",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { controller = "progress", action = "GetAllProgress" }
            );

            //config.Routes.MapHttpRoute("Progress", "Progress",
            //        new { controller = "Progress", action = "GetProgress", logging = false });

            //  config.Routes.MapHttpRoute(
            //    name: "Progression",
            //    routeTemplate: "api/{controller}/",
            //    defaults: new { controller = "Progress", action = "Index" }
            //);
        }
    }
}
