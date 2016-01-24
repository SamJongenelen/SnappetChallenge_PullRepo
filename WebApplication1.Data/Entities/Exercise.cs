using System.Collections.Generic;
using WebApplication1.Data.Entities.Base;

namespace WebApplication1.Data.Entities
{
    public class Exercise : BaseEntity
    {
        public long ExerciseId { get; set; }
        public double Difficulty { get; set; }

        public long DomainId { get; set; }
        public virtual Domain Domain { get; set; }
        public long SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public long ObjectiveId { get; set; }
        public virtual Objective Objective { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
