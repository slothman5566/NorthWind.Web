using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using System;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Northwind.Repo
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
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
            var query = _DbContext.Set<TEntity>().AsQueryable();
            if (children != null)
            {
                query = children.Aggregate(query,
                 (current, include) => current.Include(include));
            }


            if (filter == null)
            {
                return query;
            }

            return query.Where(filter);
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