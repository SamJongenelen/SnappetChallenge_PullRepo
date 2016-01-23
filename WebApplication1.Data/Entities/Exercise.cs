using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Data.Entities.Base;

namespace WebApplication1.Data.Entities
{
    public class Exercise : BaseEntity
    {
        public long DomainId { get; set; }
        public long SubjectId { get; set; }
        public long ObjectiveId { get; set; }

        //pseudo key
        public long SourceId { get; set; }

        public double Difficulty { get; set; }

        public virtual Domain Domain { get; set; }


        public virtual Subject Subject { get; set; }
        public virtual Objective Objective { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
