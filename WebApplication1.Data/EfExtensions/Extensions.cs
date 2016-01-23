using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using System;
using System.Linq;
using System.Linq.Expressions;
using WebApplication1.Data.Entities.Base;

namespace Microsoft.Data.Entity.Extensions
{
    public static class Extensions
    {
        //note: eigen implementatie van Find want EF7 is nog in preRelease en heeft geen find()...
        //zie https://github.com/aspnet/EntityFramework/issues/797

        public static TEntity Find<TEntity>(this DbSet<TEntity> set, params object[] keyValues) where TEntity : class
        {
            var context = ((IInfrastructure<IServiceProvider>)set).GetService<DbContext>();

            var entityType = context.Model.FindEntityType(typeof(TEntity));
            var key = entityType.FindPrimaryKey();

            var entries = context.ChangeTracker.Entries<TEntity>();


            entries = entries.Where(e => e.Property(key.Properties.First().Name).CurrentValue == keyValues[0]);

            // TODO: fix; index out of bounds when key not distinct ? voor nu gewoon first() want we hebben maar één FInd() method
            //var i = 0;
            //foreach (var property in key.Properties)
            //{
            //    entries = entries.Where(e => e.Property(property.Name).CurrentValue == keyValues[i]);
            //    i++;
            //}

            var entry = entries.FirstOrDefault();
            if (entry != null)
            {
                return entry.Entity;
            }

            // TODO: Build the real LINQ Expression            
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var query = set.Where((Expression<Func<TEntity, bool>>)
                Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, "Id"),
                        Expression.Constant(keyValues[0])),
                    parameter));

            // Look in the database
            return query.FirstOrDefault();
        }

        public static void DeleteAllAndSave<T>(this DbContext context) where T : BaseEntity
        {
            foreach (var p in context.Set<T>())
            {
                context.Entry(p).State = EntityState.Deleted;
            }
            context.SaveChanges();
        }
    }
}