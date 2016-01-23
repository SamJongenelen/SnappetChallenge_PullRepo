using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Data.Contexts;
using System.Linq;
using System.IO;
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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestInitialize]
        public void DeleteDBIfExists()
        {
            var folder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var filePathMdf = $"{userProfile}\\WebApplication1.Data.Contexts.SnappetContext.mdf";

            //if (File.Exists(filePathMdf))
            //{
            //    File.Delete(filePathMdf);
            //}
            //var filePathLdf = $"{userProfile}\\WebApplication1.Data.Contexts.SnappetContext_log.ldf";

            //if (File.Exists(filePathLdf))
            //{
            //    File.Delete(filePathLdf);
            //}
            //C:\Users\Sam.PCSAM\WebApplication1.Data.Contexts.SnappetContext.mdf
        }

        [TestMethod]
        public void WHEN_DbContext_started_THEN_DbContext_Domains_is_seeded()
        {
            var context = new SnappetContext();
            Assert.IsTrue(context.Domains.Any());
        }
    }
}
