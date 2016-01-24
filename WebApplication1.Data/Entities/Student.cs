using System.Collections.Generic;
using WebApplication1.Data.Entities.Base;

namespace WebApplication1.Data.Entities
{
    public class Student : BaseEntity
    {
        //public long SourceId { get; set; }

        public string Name { get; set; }

        public double AverageProgress { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
