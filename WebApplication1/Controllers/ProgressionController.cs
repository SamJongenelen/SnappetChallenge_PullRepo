using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Data.Contexts;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProgressionController : Controller
    {
        private SnappetContext _context;
        protected SnappetContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new SnappetContext();
                }
                return _context;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public List<StudentProgression> GetProgress(DateTime dateStart, DateTime dkateEnd)
        {
            var answers = Context.Answers; //todo: using or at least dispose?
            
            var nrAnswers = answers.Count();
            var nrCorrectanswer = answers.Count(a => a.Correct);
            var nrExercises = answers.Select(x => x.ExerciseId).Distinct().Count();
            var avgDifficulty = answers.Average(a => a.Exercise.Difficulty);
            var avgCorrectnessRate = (double)nrCorrectanswer / nrExercises;
            var avgProgress = answers.Average(x => x.Progress);

            var students = answers.GroupBy(a => a.Student).Select(a => a.Key).ToList();
            var nrOfStudents = students.Count();

            var progressList = new List<StudentProgression>();

            foreach (var student in students)
            {
                var progress = new StudentProgression(student.Id, student.Name);

                progress.NumberOfExercises = answers.Count(a => a.StudentId == student.Id);
                var numOfCorrectExercisesForStudent = answers.Where(a => a.StudentId == student.Id).Count(a => a.Correct);
                progress.CorrectAnswerRate = numOfCorrectExercisesForStudent / progress.NumberOfExercises;
                progress.DifficultyOfExercises = progress.NumberOfExercises > 0 ? answers.Where(a => a.StudentId == student.Id).Average(a => a.Exercise.Difficulty) : 0; //set 0 if no exercises found
                var answersForStudent = answers.Where(a => a.StudentId == student.Id);
                var avgProgressForStudent = answersForStudent.Any() ? answersForStudent.Average(a => a.Progress) : 0;
                progress.Progresss = (avgProgressForStudent - avgProgress) / avgProgress;

                progressList.Add(progress);
            }

            return progressList;
        }
    }
}