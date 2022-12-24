using Northwind.Repo;
using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Northwind.Service.IService;

namespace Northwind.Service
{

    public abstract class EntityService<TEntity> : IEntityService<TEntity> where TEntity : class
    {
        protected IUnitOfWork _UnitOfWork;
        protected IGenericRepository<TEntity> _Repository;
        public EntityService(IUnitOfWork uow,IGenericRepository<TEntity> repository)
        {
            _UnitOfWork = uow;
            _Repository = repository;
        }

        public TEntity Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var model = _Repository.Delete(entity);

            _UnitOfWork.SaveChange();
            return model;
        }


        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _Repository.DeleteRange(entities);
        }



        public TEntity Edit(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var model = _Repository.Edit(entity);

            _UnitOfWork.SaveChange();
            return model;
        }

        public TEntity Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var model = _Repository.Insert(entity);

            _UnitOfWork.SaveChange();
            return model;
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
           return _Repository.FindBy(predicate);
        }



        public IQueryable<TEntity> GetAll()
        {
            return _Repository.GetAll();
        }


        public IQueryable<TEntity> GetAllLazyLoad(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] children)
        {
            return _Repository.GetAllLazyLoad(filter, children);
        }

        #region Async

        public async Task<TEntity> EditAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var model = _Repository.Edit(entity);

          await  _UnitOfWork.SaveChangeAsync();
            return model;
        }



     
        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var model = _Repository.Insert(entity);

            await _UnitOfWork.SaveChangeAsync();
            return model;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var model = _Repository.Delete(entity);

            await _UnitOfWork.SaveChangeAsync();
            return model;
        }
        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entity");
            }

             _Repository.DeleteRange(entities);

            await _UnitOfWork.SaveChangeAsync();
          
        }


        #endregion


    }
}
