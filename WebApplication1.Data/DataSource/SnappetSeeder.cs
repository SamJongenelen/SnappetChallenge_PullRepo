using Microsoft.Data.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApplication1.Data.Contexts;
using WebApplication1.Data.Entities;
using WebApplication1.Data.Interfaces;

namespace WebApplication1.Data.DataSource
{
    public static class SnappetSeeder
    {
        private const int _iterationSize = 1000;
        private static ISnappetContext _context;
        private static Random _random = new Random();
        private static string _jsonAsString;
        private static Stopwatch _sw = new Stopwatch();
        private static int _maxNrItems;

        public static void Seed(SnappetContext context, int maxNrItems, string jsonAsString = "")
        {
            if (_context == null)
            {
                _context = context;
                _jsonAsString = jsonAsString;
                _maxNrItems = maxNrItems;
                #region todo paralell werkt niet met EF nu, misshcien later
                //todo: werkt niet; context opbouwen parallel geeft array exception. known issue :)
                //var tasks = new List<Task>();
                //tasks.Add(Task.Factory.StartNew(() => _context.Students.AddRange(GetStudents())));
                //tasks.Add(Task.Factory.StartNew(() => _context.Domains.AddRange(GetDomains())));
                //tasks.Add(Task.Factory.StartNew(() => _context.Objectives.AddRange(GetObjectives())));
                //tasks.Add(Task.Factory.StartNew(() => context.Subjects.AddRange(GetSubjects())));
                //tasks.Add(Task.Factory.StartNew(() => _context.Exercises.AddRange(GetExercises())));
                //Task.WaitAll();
                #endregion

                ConvertJson();

                SeedStudents();
                SeedDomains();
                SeedObjectives();
                SeedSubjects();
                SeedExercises();
                SeedAnswers();
            }
        }

        private static void ConvertJson()
        {
            _context.DeleteAllAndSave<Student_Answer>(); //inMemory DB issue fix bij alwaysup VSTestEngine; gooi gewoon alles weg :')

            var reader = GetReader(_jsonAsString);//moeilijke dingen, omdat VsTest via de Test assemlby binnenkomt...

            Debug.WriteLine("Starting JSON import");

            _sw.Start();

            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                int itemsDone = 0;
                var itemsToInsert = new List<Student_Answer>();

                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType != JsonToken.StartObject) continue; //skip
                    var jObject = JObject.Load(jsonReader);

                    if (itemsDone < _maxNrItems)
                    {
                        //todo: make constants ipv magicstrings
                        var studentAnswer = new Student_Answer
                        {
                            Correct = jObject["Correct"].Value<int>() == 1,
                            DateAdded = DateTime.Now,
                            Difficulty = jObject["Difficulty"].Value<string>().Equals("NULL") ? null : jObject["Difficulty"].Value<double?>(),
                            Domain = jObject["Domain"].Value<string>(),
                            ExerciseId = jObject["ExerciseId"].Value<long>(),
                            LearningObjective = jObject["LearningObjective"].Value<string>(),
                            Progress = jObject["Progress"].Value<int>(),
                            Subject = jObject["Subject"].Value<string>(),
                            SubmitDateTime = jObject["SubmitDateTime"].Value<DateTime>(),
                            SubmittedAnswerId = jObject["SubmittedAnswerId"].Value<long>(),
                            UserId = jObject["UserId"].Value<long>(),
                            Id = jObject["SubmittedAnswerId"].Value<long>()
                        };

                        itemsToInsert.Add(studentAnswer);

                        if (itemsToInsert.Count() % _iterationSize == 0)
                        {
                            _context.StudentAnswers.AddRange(itemsToInsert);
                            _context.SaveChanges();

                            Debug.WriteLine($"Saving after #{itemsDone + 1}' item");
                            itemsToInsert = new List<Student_Answer>();
                        }
                        itemsDone++;
                    }
                    else
                    {
                        break;
                    }
                }

                _context.SaveChanges(); //ok we end with another savechanges here because it prob. wont end on mod _iterationSize :)
            }

            _sw.Stop();
            Debug.WriteLine($"imported JSON in {_sw.ElapsedMilliseconds / 1000 } seconds");
        }

        private static void SeedStudents()
        {
            _sw.Restart();
            foreach (var userId in _context.StudentAnswers.Select(s => s.UserId).Distinct().ToList())
            {
                Student s = new Student();
                s.Id = userId;
                s.Name = GetStudentName(s.Id); //todo: unique name
                _context.Students.Add(s);
            }
            _context.SaveChanges();

            _sw.Stop();
            Debug.WriteLine($"seeded students in {_sw.ElapsedMilliseconds / 1000 } seconds");

        }
        private static void SeedAnswers()
        {
            _sw.Restart();
            var distinctAnswers = _context.StudentAnswers.Select(a => new { a.ExerciseId, a.UserId, a.Correct, a.SubmitDateTime, a.SubmittedAnswerId, a.Progress }).Distinct();
            var iterationsNeeded = Math.Ceiling((double)distinctAnswers.Count() / _iterationSize);

            //genereer een hoop answers en koppel ze 
            for (var iteration = 0; iteration < iterationsNeeded; iteration++)
            {
                foreach (var answer in distinctAnswers.OrderBy(a => a.SubmittedAnswerId).Skip(iteration * _iterationSize).Take(_iterationSize))
                {
                    var generatedAnswer = new Answer
                    {
                        DateAdded = DateTime.Now,
                        Correct = answer.Correct,
                        Exercise = _context.Exercises.AsNoTracking().FirstOrDefault(e => e.ExerciseId == answer.ExerciseId),
                        Progress = answer.Progress,
                        Id = answer.SubmittedAnswerId,
                        Student = _context.Students.AsNoTracking().FirstOrDefault(s => s.Id == answer.UserId),
                        SubmitDateTime = answer.SubmitDateTime
                    };
                    _context.Answers.Add(generatedAnswer);
                }
                _context.SaveChanges(); //save every n records, because no bulkinsert for EF7 yet :(

            }

            _sw.Stop();
            Debug.WriteLine($"seeded answers in {_sw.ElapsedMilliseconds / 1000 } seconds");

        }
        private static void SeedDomains()
        {
            _sw.Restart();
            foreach (var item in _context.StudentAnswers.Select(s => s.Domain).Distinct())
            {
                Domain d = new Domain();
                d.DateAdded = DateTime.Now;
                d.DomainName = item;

                _context.Domains.Add(d);
            }
            _context.SaveChanges();
            _sw.Stop();
            Debug.WriteLine($"seeded domains in {_sw.ElapsedMilliseconds / 1000 } seconds");
        }
        private static void SeedObjectives()
        {
            _sw.Restart();
            foreach (var objective in _context.StudentAnswers.Select(s => s.LearningObjective).Distinct())
            {
                Objective d = new Objective();
                d.DateAdded = DateTime.Now;
                d.LearningObjective = objective;
                _context.Objectives.Add(d);
            }
            _context.SaveChanges();
            _sw.Stop();
            Debug.WriteLine($"seeded objectives in {_sw.ElapsedMilliseconds / 1000 } seconds");
        }
        private static void SeedSubjects()
        {
            _sw.Restart();
            foreach (var subject in _context.StudentAnswers.Select(s => s.Subject).Distinct())
            {
                Subject s = new Subject();
                s.DateAdded = DateTime.Now;
                s.Name = subject;
                _context.Subjects.Add(s);
            }
            _context.SaveChanges();
            _sw.Stop();
            Debug.WriteLine($"seeded students in {_sw.ElapsedMilliseconds / 1000 } seconds");
        }
        private static void SeedExercises()
        {
            _sw.Restart();
            var distinctExercises = _context.StudentAnswers.Select(s => new { s.ExerciseId, s.Domain, s.Subject, s.Difficulty, s.LearningObjective }).Distinct();
            var iterationsNeeded = Math.Ceiling((double)distinctExercises.Count() / _iterationSize);

            for (var iteration = 0; iteration < iterationsNeeded; iteration++)
            {
                foreach (var exercise in distinctExercises.Skip(iteration * _iterationSize).Take(_iterationSize))
                {
                    Exercise e = new Exercise()
                    {
                        DateAdded = DateTime.Now,
                        Difficulty = exercise.Difficulty ?? 0,
                        Domain = _context.Domains.AsNoTracking().FirstOrDefault(d => d.DomainName == exercise.Domain),
                        Objective = _context.Objectives.AsNoTracking().FirstOrDefault(o => o.LearningObjective == exercise.LearningObjective),
                        ExerciseId = exercise.ExerciseId,
                        Subject = _context.Subjects.AsNoTracking().FirstOrDefault(s => s.Name == exercise.Subject) // name != key dus kan fouten veroorzaken, misschien SIngle() pakken?
                    };
                    _context.Exercises.Add(e);
                }
                _context.SaveChanges();
            }
            _sw.Stop();
            Debug.WriteLine($"seeded exercises in {_sw.ElapsedMilliseconds / 1000 } seconds");
        }

        private static string GetStudentName(long id)
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
        /// <summary>
        /// Needed so the VsTest engine can deliver a reader as well; fix for CI
        /// </summary>
        /// <returns></returns>
        private static TextReader GetReader(string jsonAsString)
        {
            TextReader reader;
            if (string.IsNullOrEmpty(jsonAsString))
            {
                try
                {
                    var assembly = Assembly.GetCallingAssembly();
                    var r = assembly.GetManifestResourceNames();
                    reader = new StreamReader(assembly.GetManifestResourceStream("WebApplication1.Data.DataSource.work.json")); //todo: refactor
                }
                catch
                {
                    var assembly = Assembly.GetEntryAssembly();
                    var r = assembly.GetManifestResourceNames();
                    reader = new StreamReader(assembly.GetManifestResourceStream("WebApplication1.Data.DataSource.work.json")); //todo: refactor
                }
            }
            else
            {
                reader = new StringReader(jsonAsString);
            }
            return reader;
        }
    }
}
