using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Progress
    {
        public double Progresss { get; set; }

        public double CorrectAnswerRate { get; set; }

        public double NumberOfExercises { get; set; }

        public double DifficultyOfExercises { get; set; }
    }
}
