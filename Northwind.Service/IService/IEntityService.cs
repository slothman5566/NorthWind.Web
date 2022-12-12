using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Service.IService
{
    public interface IEntityService<TEntity> where TEntity : class
    {


        IQueryable<TEntity> GetAll();

        //IQueryable<TEntity> GetAllLazyLoad(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] children);

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        TEntity Insert(TEntity entity);

        TEntity Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);

        TEntity Edit(TEntity entity);

        #region Async


        Task<TEntity> InsertAsync(TEntity entity);

        Task<TEntity> DeleteAsync(TEntity entity);

        Task DeleteRangeAsync(IEnumerable<TEntity> entities);

        Task<TEntity> EditAsync(TEntity entity);



        #endregion
    }
}
