using System.Data.Entity;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Entities;
using System;

namespace WebApplication1.Data.Contexts
{
    public class SnappetContext : DbContext, ISnappetContext
    {
        public SnappetContext() : base($"Data Source = (LocalDB)\v11.0; AttachDbFilename={ Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\WebApplication1.Data.Contexts.SnappetContext.mdf;Integrated Security = True")
        {
            Database.SetInitializer(new Snappet_DbInitializer());  //create DB and seed
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        /// DBSETS
        public DbSet<Student_Answer> StudentAnswers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Objective> Objectives { get; set; }
    }
}
