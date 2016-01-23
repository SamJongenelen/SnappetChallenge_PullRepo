using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Entities;
using Microsoft.Data.Entity;
using System;
using Microsoft.Data.Entity.Infrastructure;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data.DataSource;
using WebApplication1.Data.Entities.Base;
using Microsoft.Data.Entity.Extensions;
using System.Reflection;

namespace WebApplication1.Data.Contexts
{
    public class SnappetContext : DbContext, ISnappetContext
    {
        public SnappetContext(DbContextOptions dbContextOptions, int maxNrItems = 10000000) : base(dbContextOptions)
        {
            //helaas nog geen Seed() ook in EF7, dus maar ff met het handje, omdat ik geen SQLServer instance heb op mijn home PC (en deployment en demo een stuk moeilijker wordt...)
            //https://github.com/aspnet/EntityFramework/issues/629

            ConvertJsonToDbContext(this, maxNrItems);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Fluent mappings: (data annotations met FK en many to many is lastig en fluent is de way to go volgens hanselman
            modelBuilder.Entity<Student>().HasMany<Answer>().WithOne(x => x.Student);

            modelBuilder.Entity<Answer>().HasOne<Student>().WithMany(x => x.Answers);
            modelBuilder.Entity<Answer>().HasOne(x => x.Student).WithMany(ex => ex.Answers).HasForeignKey(answer => answer.ExerciseId);

            //Exercise:
            modelBuilder.Entity<Exercise>().HasOne<Subject>().WithMany(x => x.Exercises);
            modelBuilder.Entity<Exercise>().HasOne<Objective>().WithMany(x => x.Exercises);
            modelBuilder.Entity<Exercise>().HasMany<Answer>().WithOne(x => x.Exercise).HasForeignKey(x => x.SourceId); //todo: check Fk?

            //modelBuilder.Entity<Answer>().HasOne(answer => answer.Student).WithOne(y => y.);

            //modelBuilder.Entity<Answer>().HasRequired(answer => answer.Exercise).WithMany(exercise => exercise.Answers).HasForeignKey(answer => answer.ExerciseId);
        }

        private void ConvertJsonToDbContext(SnappetContext context, int maxNrItems)
        {
            //todo: get resource from another location. I want to use an Embedded Resource now because it gets the job done
            //todo: doesnt work with VSO build agent grr
            //var resourceAsByteArray = Properties.Resources.work; //geen moeilijke dingen, we weten waar de file staat en wat de content is dus no nonsense here.

            var assembly = Assembly.GetCallingAssembly();
            
            var resourceName = "WebApplication1.Data.DataSource.work.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                var itemsToInsert = new List<Student_Answer>();
                {
                    while (jsonReader.Read())
                    {
                        if (jsonReader.TokenType != JsonToken.StartObject) continue;
                        var obj = JObject.Load(jsonReader);

                        if (itemsToInsert.Count < maxNrItems)
                        {
                            //todo: make constants ipv magicstrings
                            var studentAnswer = new Student_Answer
                            {
                                Correct = obj["Correct"].Value<int>() == 1,
                                DateAdded = DateTime.Now,
                                Difficulty = obj["Difficulty"].Value<string>().Equals("NULL") ? null : obj["Difficulty"].Value<double?>(),
                                Domain = obj["Domain"].Value<string>(),
                                ExerciseId = obj["ExerciseId"].Value<long>(),
                                LearningObjective = obj["LearningObjective"].Value<string>(),
                                Progress = obj["Progress"].Value<int>(),
                                Subject = obj["Subject"].Value<string>(),
                                SubmitDateTime = obj["SubmitDateTime"].Value<DateTime>(),
                                SubmittedAnswerId = obj["SubmittedAnswerId"].Value<long>(),
                                UserId = obj["UserId"].Value<long>(),
                                Id = obj["SubmittedAnswerId"].Value<long>()
                            };

                            itemsToInsert.Add(studentAnswer);
                        }
                    }

                    context.DeleteAll<Student_Answer>(); //inMemory DB issue fix; gooi gewoon alles weg :)
                    context.StudentAnswers.AddRange(itemsToInsert);
                }

                context.SaveChanges(); //SaveChanges() also needed for InMemory Provider, to make the properties able to navigate 

                SnappetCustomSeeder seeder = new SnappetCustomSeeder(ref context); //seed data based on the Student_Answer


            }
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
