using System.Linq;
using System.Web.Mvc;
using WebApplication1.Data.Contexts;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StudentsController : Controller
    {
        //[Route("students")] //todo: annotation routes zijn sjiek later ff naar kijken
        public ActionResult Index()
        {
            //todo: add service which gets the data for model? even better would be client js framework to get from REST service
            StudentsModel model = GetStudents();
            return View(model);
        }

        //todo: refactor to service
        private StudentsModel GetStudents()
        {
            using (var context = new SnappetContext())
            {
                var studentLIst = context.Students.ToList();
                return new StudentsModel(studentLIst);
            }
        }
    }
}