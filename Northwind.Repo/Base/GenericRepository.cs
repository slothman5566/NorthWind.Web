using Microsoft.EntityFrameworkCore;
using NorthWind.Data;

namespace Northwind.Repo
{
    public abstract class GenericRepository<TEntity>:IGenericRepository<TEntity> where TEntity : class
    {
        protected DbContext _DbContext { get; set; }
        public GenericRepository(DbContext dbContext)
        {
            _DbContext = dbContext;
        }
        public TEntity Find(object id)
        {
            return _DbContext.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {

            return _DbContext.Set<TEntity>();
        }

        public TEntity Insert(TEntity entity)
        {
           return _DbContext.Set<TEntity>().Add(entity).Entity;
        }
    }
}