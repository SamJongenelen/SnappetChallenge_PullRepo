using System;
using WebApplication1.Data.Entities.Base;

namespace WebApplication1.Data.Entities
{
    public class Answer : BaseEntity
    {
        public long SourceId { get; set; }
        public bool Correct { get; set; }
        public DateTime SubmitDateTime { get; set; }
        public double Progress { get; set; }

        public long ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }

        public long StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
