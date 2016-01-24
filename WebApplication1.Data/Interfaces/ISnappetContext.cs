using Microsoft.Data.Entity;
using WebApplication1.Data.Entities;
using WebApplication1.Data.Entities.Base;

namespace WebApplication1.Data.Interfaces
{
    public interface ISnappetContext : IDbContext
    {
        DbSet<Student_Answer> StudentAnswers { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<Subject> Subjects { get; set; }
        DbSet<Domain> Domains { get; set; }
        DbSet<Exercise> Exercises { get; set; }
        DbSet<Answer> Answers { get; set; }
        DbSet<Objective> Objectives { get; set; }
    }

    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;
        int SaveChanges();
        void Dispose();
        void DeleteAllAndSave<T>() where T : BaseEntity;
    }
}
