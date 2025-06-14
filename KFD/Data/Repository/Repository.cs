using System.Linq.Expressions;
using KFD.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KFD.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (includeProperties != null)
                foreach (var p in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(p);
                }
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (includeProperties != null)
                foreach (var p in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(p);
                }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
