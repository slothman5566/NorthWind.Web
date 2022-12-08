using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using System;
using System.Linq.Expressions;

namespace Northwind.Repo
{
    public abstract class GenericRepository<TEntity>:IGenericRepository<TEntity> where TEntity : class
    {
        protected DbContext _DbContext { get; set; }
        public GenericRepository(DbContext dbContext)
        {
            _DbContext = dbContext;
           
        }

     

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _DbContext.Set<TEntity>().Where(predicate).AsQueryable();
        }

        IQueryable<TEntity> IGenericRepository<TEntity>.GetAll()
        {
            return _DbContext.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> GetAllLazyLoad(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] children)
        {
            children.ToList().ForEach(_ => _DbContext.Set<TEntity>().Include(_).Load());
            if (filter == null)
            {
                return _DbContext.Set<TEntity>();
            }

            return _DbContext.Set<TEntity>().Where(filter);
        }

        public TEntity Insert(TEntity entity)
        {
            return _DbContext.Set<TEntity>().Add(entity).Entity;
        }

        public TEntity Delete(TEntity entity)
        {
          
            return _DbContext.Set<TEntity>().Remove(entity).Entity;
        }
        public void DeleteRange(IEnumerable<TEntity> entities)
        {

             _DbContext.Set<TEntity>().RemoveRange(entities);
        }

        public TEntity Edit(TEntity entity)
        {
            _DbContext.Set<TEntity>().Entry(entity).State = EntityState.Modified;
            return entity;
        }

    }
}