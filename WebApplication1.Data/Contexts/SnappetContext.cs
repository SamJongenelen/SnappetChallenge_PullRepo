using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Entities;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using WebApplication1.Data.DataSource;
using WebApplication1.Data.Entities.Base;

namespace WebApplication1.Data.Contexts
{
    public class SnappetContext : DbContext, ISnappetContext
    {
        public static DbContextOptions DbContextOptions
        {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<SnappetContext>();
                optionsBuilder.UseInMemoryDatabase();

                return optionsBuilder.Options;
            }
            set { }
        }

        public SnappetContext(string _jsonAsString = "", int maxNrItems = 1000) : this(DbContextOptions, _jsonAsString) { }

        public SnappetContext(DbContextOptions dbContextOptions, string jsonAsString = "") : base(dbContextOptions)
        {
            //notes:
            // - jsonAsString even snel toegevoegd aan constructor, om tests snel mogelijk te maken
            // - helaas nog geen Seed() ook in EF7, dus maar ff manual vullen met een static seeder (zodat ik gewoon InMemory provider kan gebruiken)
            // zie https://github.com/aspnet/EntityFramework/issues/629
#if DEBUG
            SnappetSeeder.Seed(this, 1000000, jsonAsString);
#else
            SnappetSeeder.Seed(this, 1000, jsonAsString); //todo: increase for production
#endif 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //EF7 Fluent mappings: (data annotations met FK en many to many is lastig en fluent is the way to go volgens hanselman). 
            //Dit zijn de enige die ik moet maken, de rest doet EF zelf voor me
            modelBuilder.Entity<Answer>().HasOne<Student>().WithMany(x => x.Answers);
            modelBuilder.Entity<Answer>().HasOne(x => x.Student).WithMany(ex => ex.Answers).HasForeignKey(answer => answer.ExerciseId);
        }

        /// DBSETS
        public DbSet<Student_Answer> StudentAnswers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Objective> Objectives { get; set; }

        public void DeleteAllAndSave<T>() where T : BaseEntity
        {
            foreach (var p in Set<T>())
            {
                Entry(p).State = EntityState.Deleted;
            }
            SaveChanges();
        }
    }
}
