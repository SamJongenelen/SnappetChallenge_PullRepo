using System.Linq;
using System.Web.Mvc;
using WebApplication1.Data.Contexts;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StudentsController : Controller
    {
        private SnappetContext _context;
        protected SnappetContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new SnappetContext();
                }
                return _context;
            }
        }

        //[Route("students")] //todo: annotation routes zijn sjiek later configureren if needed
        public ActionResult Index()
        {
            //todo: add service which gets the data for model? even better would be client js framework to get from REST service
            StudentsModel model = GetStudents();
            return View(model);
        }

        //todo: refactor to service
        private StudentsModel GetStudents()
        {
            var studentList = Context.Students.ToList();
            return new StudentsModel(studentList);
        }
    }
}