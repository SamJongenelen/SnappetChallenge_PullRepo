using System.Collections.Generic;
using WebApplication1.Data.Entities.Base;

namespace WebApplication1.Data.Entities
{
    public class Exercise : BaseEntity
    {
        public long DomainId { get; set; }
        public long SubjectId { get; set; }
        public long ObjectiveId { get; set; }

        // ID in the original data. Because of the usage of an identity key we cannot set this value as a key but we store it for reference
        public long SourceId { get; set; }

        public double Difficulty { get; set; }
               
        
        //navigational props 
        public virtual Subject Subject { get; set; }

        public virtual Objective Objective { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
