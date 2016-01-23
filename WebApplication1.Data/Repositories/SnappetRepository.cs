using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Extensions;
using System.Linq;
using WebApplication1.Data.Entities;
using WebApplication1.Data.Entities.Base;
using WebApplication1.Data.Interfaces;

namespace WebApplication1.Data.Contexts
{
    //Repository pattern om te kunnen faken/moqen in een later punt

    public class SnappetRepository<T> where T : BaseEntity
    {
        private ISnappetContext _context;
        private DbSet<T> _dbSet;

        public SnappetRepository(ISnappetContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual T GetById(long id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public virtual T GetBySourceId(long id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        //1038396

        public virtual void Insert(T entity)
        {
            _dbSet.Add(entity);
        }
    }
}

