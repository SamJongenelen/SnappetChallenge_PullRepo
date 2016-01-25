using System.Collections.Generic;
using WebApplication1.Data.Entities;

namespace WebApplication1.Models
{
    public class StudentsModel : List<Student>
    {
        public StudentsModel(IEnumerable<Student> collection) : base(collection) { }
    }
}
