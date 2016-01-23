using System.Collections.Generic;
using WebApplication1.Data.Entities.Base;

namespace WebApplication1.Data.Entities
{
    public class Objective : BaseEntity
    {
        public string LearningObjective { get; set; }

        public virtual ICollection<Exercise> Exercises { get; set; }
    }
}
