using System.Collections.Generic;
using WebApplication1.Data.Entities.Base;

namespace WebApplication1.Data.Entities
{
    public class Subject : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
