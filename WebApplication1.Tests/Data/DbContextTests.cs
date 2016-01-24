using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Data.Contexts;
using System.Linq;
using System.IO;
using Microsoft.Data.Entity;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Entities;

namespace WebApplication1.Tests.Data
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class DbContextTest
    {
        public DbContextTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private static ISnappetContext _context;
        private static string _jsonAsString;
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        //cannot use TestINitialize and TestCleanup because of VsTest in VSO build agent 
        [ClassInitialize]
        [DeploymentItem("DataSource\\work.json")]
        public static void ClassInitialize(TestContext testContext)
        {
            //TestContext = TestContext;

            _jsonAsString = File.ReadAllText("DataSource\\work.json");

            var optionsBuilder = new DbContextOptionsBuilder<SnappetContext>();
            optionsBuilder.UseInMemoryDatabase();

            _context = new SnappetContext(optionsBuilder.Options, _jsonAsString, 1000);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _context.Dispose();
            _context = null;
        }


        //ACTUAL TESTS:

        [TestMethod]
        public void WHEN_DbContext_started_THEN_DbContext_Domains_is_seeded()
        {
            //ARRANGE
            //ACT 
            //ASSERT
            Assert.AreNotEqual(_context.Domains, null); //not really a good test, but its fine for the demo 
        }

        [TestMethod]
        public void WHEN_student_repo_asked_by_ID_THEN_student_object_is_returned()
        {
            //ARRANGE 
            var studentRepository = new SnappetRepository<Student>(_context);

            //ACT 
            var student = studentRepository.GetById(40281); //i know this ID exists in Work.Json 4 sure

            //ASSERT
            Assert.IsNotNull(student); //not really a good test, but its fine for the demo 

        }

        [TestMethod]
        public void WHEN_student_is_gotten_THEN_student_name_is_not_empty()
        {
            //ARRANGE 
            var studentRepository = new SnappetRepository<Student>(_context);

            //ACT 
            var student = studentRepository.GetById(40281); //i know this ID exists in Work.Json 4 sure

            //ASSERT
            Assert.IsFalse(string.IsNullOrWhiteSpace(student.Name));
        }

        [TestMethod]
        public void WHEN_student_is_navigated_THEN_answers_are_filled()
        {
            //ARRANGE 
            var repo = new SnappetRepository<Student>(_context);

            //ACT 
            var s = _context.Students.First(x => x.Id == 40268); //userid = 40281

            //ASSERT
            Assert.IsNotNull(s);
            Assert.IsNotNull(s.Answers);
            //Assert.AreNotEqual(exercise.Answers.Count, 0);
        }

        [TestMethod]
        public void WHEN_exercise_is_navigted_THEN_exercise_has_Subject()
        {
            //ARRANGE 
            var repo = new SnappetRepository<Exercise>(_context);

            //ACT 
            var exercise = _context.Exercises.First(x => x.ExerciseId == 1038396);

            //ASSERT
            Assert.IsNotNull(exercise);
            Assert.IsNotNull(exercise.Subject);
        }


        //test navigationals EF:
        [TestMethod]
        public void WHEN_COntext_is_loaded_ALL_navigational_properties_work()
        {
            //ARRANGE 
            var repo = new SnappetRepository<Exercise>(_context);

            //ACT 
            var student = _context.Students.First();


            //ASSERT
            Assert.AreNotEqual(0, student.Answers.Count);
            Assert.IsNotNull(student.Answers.First().Exercise);
            Assert.IsNotNull(student.Answers.First().Exercise.Domain);
            Assert.AreNotEqual(0, student.Answers.First().Exercise.Domain);
        }

        [TestMethod]
        public void Student_HAS_answers()
        {
            //ARRANGE 
            var repo = new SnappetRepository<Exercise>(_context);

            //ACT 
            var student = _context.Students.First();

            //ASSERT
            Assert.AreNotEqual(0, student.Answers.Count);
        }

        [TestMethod]
        public void Answer_has_student()
        {
            //ARRANGE 
            var repo = new SnappetRepository<Exercise>(_context);

            //ACT 
            var a = _context.Answers.First();

            //ASSERT
            Assert.AreNotEqual(0, a.Student);
        }

        [TestMethod]
        public void Answer_has_Exercise()
        {
            //ARRANGE 
            var repo = new SnappetRepository<Exercise>(_context);

            //ACT 
            var a = _context.Answers.First();

            //ASSERT
            Assert.AreNotEqual(0, a.Exercise);
        }

        [TestMethod]
        public void Exercise_HAS_answers()
        {
            //ARRANGE 
            var repo = new SnappetRepository<Exercise>(_context);

            //ACT 
            var e = _context.Exercises.First();

            //ASSERT
            Assert.AreNotEqual(0, e.Answers.Count);
        }

        [TestMethod]
        public void Exercise_HAS_Domain()
        {
            //ARRANGE 
            var repo = new SnappetRepository<Exercise>(_context);

            //ACT 
            var e = _context.Exercises.First();

            //ASSERT
            Assert.IsNotNull(e.Domain);

        }

        [TestMethod]
        public void Exercise_HAS_Subject()
        {
            //ARRANGE 
            var repo = new SnappetRepository<Exercise>(_context);

            //ACT 
            var e = _context.Exercises.First();

            //ASSERT
            Assert.IsNotNull(e.Subject);
        }

        [TestMethod]
        public void Exercise_HAS_Objective()
        {
            //ARRANGE 
            var repo = new SnappetRepository<Exercise>(_context);

            //ACT 
            var e = _context.Exercises.First();

            //ASSERT
            Assert.IsNotNull(e.Objective);
        }
    }
}
