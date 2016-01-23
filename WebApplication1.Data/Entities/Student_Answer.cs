using System;
using WebApplication1.Data.Entities.Base;

namespace WebApplication1.Data.Entities
{
    public class Student_Answer : BaseEntity
    {
        public long SubmittedAnswerId { get; set; }
        public DateTime SubmitDateTime { get; set; }
        public bool Correct { get; set; }
        public int Progress { get; set; }

        public long UserId { get; set; }

        public long ExerciseId { get; set; }
        public double? Difficulty { get; set; }
        public string Subject { get; set; }
        public string Domain { get; set; }
        public string LearningObjective { get; set; }
    }
}
