using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Entities;
using Microsoft.Data.Entity;
using System;
using Microsoft.Data.Entity.Infrastructure;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using WebApplication1.Data.DataSource;
using Microsoft.Data.Entity.Extensions;
using System.Reflection;

namespace WebApplication1.Data.Contexts
{
    public class SnappetContext : DbContext, ISnappetContext
    {
        public SnappetContext(DbContextOptions dbContextOptions, string jsonAsString = "", int maxNrItems = 10000000) : base(dbContextOptions)
        {
            //notes:
            // - jsonAsString even snel toegevoegd aan constructor, om tests snel mogelijk te maken
            // - helaas nog geen Seed() ook in EF7, dus maar ff manual vullen (zodat ik gewoon InMemory provider kan gebruiken)
            //https://github.com/aspnet/EntityFramework/issues/629

            ConvertJsonToDbContext(maxNrItems, jsonAsString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //EF7 Fluent mappings: (data annotations met FK en many to many is lastig en fluent is the way to go volgens hanselman)
                      
            modelBuilder.Entity<Answer>().HasOne<Student>().WithMany(x => x.Answers);
            modelBuilder.Entity<Answer>().HasOne(x => x.Student).WithMany(ex => ex.Answers).HasForeignKey(answer => answer.ExerciseId);

           
        }

        private void ConvertJsonToDbContext(int maxNrItems, string jsonAsString = "")
        {
            //geen moeilijke dingen, we weten waar de file staat en wat de content is dus no nonsense here.
            var assembly = Assembly.GetCallingAssembly();
            var resourceName = "WebApplication1.Data.DataSource.work.json";

            //decide what baseclass reader to get (if Test or Web mode), making sure VSO agent builds and tests correctly...
            TextReader reader = string.IsNullOrEmpty(jsonAsString) ? new StreamReader(assembly.GetManifestResourceStream(resourceName)) : reader = new StringReader(jsonAsString);
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

                    this.DeleteAllAndSave<Student_Answer>(); //inMemory DB issue fix bij alwaysup VSTestEngine; gooi gewoon alles weg :')
                    StudentAnswers.AddRange(itemsToInsert);
                }

                SaveChanges(); //also needed for InMemory Provider

                SnappetCustomSeeder seeder = new SnappetCustomSeeder(this); //seed data based on the Student_Answer
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
