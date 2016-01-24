using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    [RoutePrefix("api")]
    public class ProgressController : ApiController
    {
  
        [HttpGet]
        public object GetAllProgress()
        {
            return new Progress();
        }
    }
}
