namespace WebApplication1.MVC6Web.Controllers
{
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Boilerplate.Web.Mvc;
    using Boilerplate.Web.Mvc.Filters;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.OptionsModel;
    using WebApplication1.MVC6Web.Constants;
    using WebApplication1.MVC6Web.Services;
    using WebApplication1.MVC6Web.Settings;

    public class HomeController : Controller
    {
        #region Fields

        private readonly IOptions<AppSettings> appSettings;
        private readonly IBrowserConfigService browserConfigService;

        #endregion

        #region Constructors

        public HomeController(
            IBrowserConfigService browserConfigService,
            IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings;
            this.browserConfigService = browserConfigService;
        }

        #endregion

        [HttpGet("", Name = HomeControllerRoute.GetIndex)]
        public IActionResult Index()
        {
            return this.View(HomeControllerAction.Index);
        }

        [HttpGet("about", Name = HomeControllerRoute.GetAbout)]
        public IActionResult About()
        {
            return this.View(HomeControllerAction.About);
        }

        /// <summary>
        /// Gets the browserconfig XML for the current site. This allows you to customize the tile, when a user pins 
        /// the site to their Windows 8/10 start screen. See http://www.buildmypinnedsite.com and 
        /// https://msdn.microsoft.com/en-us/library/dn320426%28v=vs.85%29.aspx
        /// </summary>
        /// <returns>The browserconfig XML for the current site.</returns>
        [NoTrailingSlash]
        [ResponseCache(CacheProfileName = CacheProfileName.BrowserConfigXml)]
        [Route("browserconfig.xml", Name = HomeControllerRoute.GetBrowserConfigXml)]
        public ContentResult BrowserConfigXml()
        {
            string content = this.browserConfigService.GetBrowserConfigXml();
            return this.Content(content, ContentType.Xml, Encoding.UTF8);
        }
    }
}
