namespace WebApplication1.MVC6Web
{
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.CookiePolicy;
    using Microsoft.AspNet.Hosting;

    public partial class Startup
    {
        /// <summary>
        /// Configure default cookie settings for the application which are more secure by default.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="environment">The environment the application is running under. This can be Development, 
        /// Staging or Production by default.</param>
        private static void ConfigureCookies(
            IApplicationBuilder application,
            IHostingEnvironment environment)
        {
            application.UseCookiePolicy(
                new CookiePolicyOptions()
                {
                    // Ensure that external script cannot access the cookie.
                    HttpOnly = HttpOnlyPolicy.Always,
                    // Ensure that the cookie can only be transported over the same scheme as the request.
                    Secure = SecurePolicy.SameAsRequest
                });
        }
    }
}
