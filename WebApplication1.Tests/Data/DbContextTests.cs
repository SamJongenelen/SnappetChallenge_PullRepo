using System;
using System.Text;
using System.Collections.Generic;
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
    public class DbContextTests
    {
        public DbContextTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private ISnappetContext _context;

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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

        [TestInitialize]
        public void CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SnappetContext>();
            optionsBuilder.UseInMemoryDatabase();

            _context = new SnappetContext(optionsBuilder.Options, 1000);
        }

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
            var exercise = _context.Exercises.First(x => x.SourceId == 1038396);

            //ASSERT
            Assert.IsNotNull(exercise);
            Assert.IsNotNull(exercise.Subject);
            //Assert.AreNotEqual(exercise.Answers.Count, 0);
        }
    }
}
