namespace WebApplication1.Models
{
    public class StudentProgression
    {
        public StudentProgression(long id, string name)
        {
            StudentId = id;
            StudentName = name;
        }

        public long StudentId { get; set; }

        public string StudentName { get; set; }

        public double Progresss { get; set; }

        public double CorrectAnswerRate { get; set; }

        public double NumberOfExercises { get; set; }

        public double DifficultyOfExercises { get; set; }
    }
}
