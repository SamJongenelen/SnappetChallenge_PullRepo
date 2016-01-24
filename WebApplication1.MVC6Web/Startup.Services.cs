namespace WebApplication1.MVC6Web
{
    using Microsoft.Extensions.DependencyInjection;
    using WebApplication1.MVC6Web.Services;

    public partial class Startup
    {
        /// <summary>
        /// Configures custom services to add to the ASP.NET MVC 6 Injection of Control (IoC) container.
        /// </summary>
        /// <param name="services">The services collection or IoC container.</param>
        private static void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddScoped<IBrowserConfigService, BrowserConfigService>();

            // Add your own custom services here e.g.

            // Singleton - Only one instance is ever created and returned.
            // services.AddSingleton<IExampleService, ExampleService>();

            // Scoped - A new instance is created and returned for each request/response cycle.
            // services.AddScoped<IExampleService, ExampleService>();

            // Transient - A new instance is created and returned each time.
            // services.AddTransient<IExampleService, ExampleService>();
        }
    }
}
