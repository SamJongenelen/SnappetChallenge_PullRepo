using Microsoft.Data.Entity;
using System.Data.Entity.Infrastructure;
using WebApplication1.Data.Entities;
using WebApplication1.Data.Entities.Base;

namespace WebApplication1.Data.Interfaces
{
    //todo, maar demo purposes; moq/fak able interfacing voor de context 
    public interface ISnappetContext : IDbContext
    {
        //pluralization of sets
        DbSet<Student_Answer> StudentAnswers { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<Subject> Subjects { get; set; }
        DbSet<Domain> Domains { get; set; }
        DbSet<Exercise> Exercises { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<Objective> Objectives { get; set; }
    }

    // basic stuff for DBContext (at this point im not sure what the underlying db will be, if possible ill try in memory. 
    // I think thats only support in EF7 but ill init some lists from there data for demo purposes

    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        int SaveChanges();
        void Dispose();

    }
}
