namespace WebApplication1.MVC6Web
{
    using System.Collections.Generic;
    using Boilerplate.Web.Mvc.Filters;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Mvc.Filters;
    using Microsoft.AspNet.Hosting;
    using Microsoft.AspNet.Routing;
    using WebApplication1.MVC6Web.Constants;

    public partial class Startup
    {
        /// <summary>
        /// Adds filters which help improve security.
        /// </summary>
        /// <param name="environment">The environment the application is running under. This can be Development, 
        /// Staging or Production by default.</param>
        private static void ConfigureSecurityFilters(IHostingEnvironment environment, ICollection<IFilterMetadata> filters)
        {
        }
    }
}
