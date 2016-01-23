using System.Collections.Generic;
using WebApplication1.Data.Entities.Base;

namespace WebApplication1.Data.Entities
{
    public class Domain : BaseEntity
    {
        public string DomainName { get; set; }

        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
