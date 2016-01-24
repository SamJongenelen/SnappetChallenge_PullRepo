using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Contexts;
using WebApplication1.Data.Entities;
using WebApplication1.Data.Interfaces;

namespace WebApplication1.Data.DataSource
{
    public class SnappetCustomSeeder
    {
        private ISnappetContext _context;
        private Random _random = new Random();

        public SnappetCustomSeeder(SnappetContext context)
        {
            _context = context;

            #region todo paralell
            //todo: werkt niet; context opbouwen parallel geeft array exception. known issue :)
            //var tasks = new List<Task>();
            //tasks.Add(Task.Factory.StartNew(() => _context.Students.AddRange(GetStudents())));
            //tasks.Add(Task.Factory.StartNew(() => _context.Domains.AddRange(GetDomains())));
            //tasks.Add(Task.Factory.StartNew(() => _context.Objectives.AddRange(GetObjectives())));
            //tasks.Add(Task.Factory.StartNew(() => context.Subjects.AddRange(GetSubjects())));
            //tasks.Add(Task.Factory.StartNew(() => _context.Exercises.AddRange(GetExercises())));
            //Task.WaitAll();
            #endregion

            //Create other entities based on the studnert answer koppeltabel:
            _context.Students.AddRange(GetStudents());
            _context.SaveChanges();

            _context.Domains.AddRange(GetDomains());
            _context.SaveChanges();

            _context.Objectives.AddRange(GetObjectives());
            _context.SaveChanges();

            _context.Subjects.AddRange(GetSubjects());
            _context.SaveChanges();

            _context.Exercises.AddRange(GetExercises());
            _context.SaveChanges();

            _context.Answers.AddRange(GetAnswers());

            _context.SaveChanges();
            // TODO: refactor zodat saveChanges niet telkens called hoeft te worden, haha (inject de context?)

        }

        private List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            foreach (var userId in _context.StudentAnswers.Select(s => s.UserId).Distinct().ToList())
            {
                Student s = new Student();
                s.Id = userId;
                s.Name = GetStudentName(s.Id); //todo: unique name
                students.Add(s);
            }

            return students;
        }

        private List<Answer> GetAnswers()
        {
            var answers = new List<Answer>();
            var distinctAnswers = _context.StudentAnswers.Select(a => new { a.ExerciseId, a.UserId, a.Correct, a.SubmitDateTime, a.SubmittedAnswerId, a.Progress }).Distinct();

            //genereer een hoop answers en koppel ze 
            foreach (var answer in distinctAnswers.OrderBy(a => a.SubmittedAnswerId))
            {
                var generatedAnswer = new Answer
                {
                    DateAdded = DateTime.Now,
                    Correct = answer.Correct,
                    Exercise = _context.Exercises.FirstOrDefault(e => e.ExerciseId == answer.ExerciseId),
                    Progress = answer.Progress,
                    Id = answer.SubmittedAnswerId,
                    Student = _context.Students.FirstOrDefault(s => s.Id == answer.UserId),
                    SubmitDateTime = answer.SubmitDateTime
                };
                answers.Add(generatedAnswer);
            }


            return answers;
        }

        private string GetStudentName(long id)
        {
            var mod = id % 4;

            if (mod < 10)
            {
                return $"Janssen { _random.Next(10000)}";
            }
            else if (mod > 10 && mod < 100)
            {
                return $"Steur { _random.Next(10000)}";
            }
            else
            {
                return $"De vries { _random.Next(10000)}";
            }
        }

        private List<Domain> GetDomains()
        {
            List<Domain> domains = new List<Domain>();

            foreach (var item in _context.StudentAnswers.Select(s => s.Domain).Distinct())
            {
                Domain d = new Domain();
                d.DateAdded = DateTime.Now;
                d.DomainName = item;
                domains.Add(d);
            }

            return domains;
        }
        private List<Objective> GetObjectives()
        {
            List<Objective> objectives = new List<Objective>();

            foreach (var objective in _context.StudentAnswers.Select(s => s.LearningObjective).Distinct())
            {
                Objective d = new Objective();
                d.DateAdded = DateTime.Now;
                d.LearningObjective = objective;
                objectives.Add(d);
            }

            return objectives;
        }
        private List<Subject> GetSubjects()
        {
            List<Subject> subjects = new List<Subject>();

            foreach (var subject in _context.StudentAnswers.Select(s => s.Subject).Distinct())
            {
                Subject s = new Subject();
                s.DateAdded = DateTime.Now;
                s.Name = subject;
                subjects.Add(s);
            }

            return subjects;
        }
        private List<Exercise> GetExercises()
        {
            List<Exercise> exercises = new List<Exercise>();

            foreach (var exercise in _context.StudentAnswers.Select(s => new { s.ExerciseId, s.Domain, s.Subject, s.Difficulty, s.LearningObjective }).Distinct())
            {
                Exercise e = new Exercise()
                {
                    DateAdded = DateTime.Now,
                    Difficulty = exercise.Difficulty ?? 0,
                    Domain = _context.Domains.FirstOrDefault(d => d.DomainName == exercise.Domain),
                    Objective = _context.Objectives.FirstOrDefault(o => o.LearningObjective == exercise.LearningObjective),
                    ExerciseId = exercise.ExerciseId,
                    Subject = _context.Subjects.FirstOrDefault(s => s.Name == exercise.Subject) // name != key dus kan fouten veroorzaken, misschien SIngle() pakken?
                };
                exercises.Add(e);
            }

            return exercises;
        }
    }
}
