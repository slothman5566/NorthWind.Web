using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.RepoV1.Base
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// For分次查詢用
        /// </summary>
        /// <returns></returns>
        Expression<Func<TEntity, bool>> GetPredicate();

        #region Create

        void Create(TEntity entity);

        Task CreateAsync(TEntity entity);

        void CreateRange(IEnumerable<TEntity> entities);

        Task CreateRangeAsync(IEnumerable<TEntity> entities);

        #endregion

        #region Delete

        void Delete(TEntity entity);


        void DeleteRange(IEnumerable<TEntity> entities);
        /// <summary>
        /// delete(query.Where(predicate))
        /// </summary>
        /// <param name="predicate"></param>
        void DeleteRange(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region Update

        void Update(TEntity entity);

        #endregion

        #region Getter

        #region GetAll

        IEnumerable<TEntity> GetAll();
        /// <summary>
        /// query.Skip(page*limit).Take(limit)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(int? page, int? limit);
        /// <summary>
        /// query.Where(predicate)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// query.Where(predicate).Skip(page*limit).Take(limit)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, int? page, int? limit);
        /// <summary>
        /// query.Where(predicate).Include(includes)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        /// <summary>
        /// query.Where(predicate).Include(includes).Skip(page*limit).Take(limit)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, int? page, int? limit, params Expression<Func<TEntity, object>>[] includes);

        #endregion

        #region GetAllAsync

        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// query.Skip(page*limit).Take(limit)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(int? page, int? limit);
        /// <summary>
        /// query.Where(predicate)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// query.Where(predicate).Skip(page*limit).Take(limit)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, int? page, int? limit);
        /// <summary>
        /// query.Where(predicate).Include(includes)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        /// <summary>
        /// query.Where(predicate).Include(includes).Skip(page*limit).Take(limit)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, int? page, int? limit, params Expression<Func<TEntity, object>>[] includes);

        #endregion

        #region GetFirst
        TEntity GetFirst();
        /// <summary>
        /// query.Where(predicate).FirstOrDefault
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity GetFirst(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// query.Where(predicate).Include(includes).FirstOrDefault
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        TEntity GetFirst(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        #endregion

        #region GetFirstAsync

        Task<TEntity> GetFirstAsync();
        /// <summary>
        /// query.Where(predicate).FirstOrDefault
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// query.Where(predicate).Include(includes).FirstOrDefault
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        #endregion

        #region GetLast
        TEntity GetLast();

        /// <summary>
        /// query.Where(predicate).LastOrDefault
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity GetLast(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// query.Where(predicate).Include(includes).LastOrDefault
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        TEntity GetLast(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        #endregion

        #region GetLastAsync

        Task<TEntity> GetLastAsync();

        /// <summary>
        /// query.Where(predicate).LastOrDefault
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> GetLastAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// query.Where(predicate).Include(includes).LastOrDefault
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<TEntity> GetLastAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        #endregion

        #endregion

        #region Select

        #region Select
        /// <summary>
        /// query.Select(selector)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector);
        /// <summary>
        /// query.Where(predicate).Select(selector)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector);
        /// <summary>
        ///  query.Where(predicate).Skip(page*limit).Take(limit).Select(selector)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, bool>> predicate, int? page, int? limit, Expression<Func<TEntity, TResult>> selector);

        #endregion

        #region SelectAsync

        /// <summary>
        /// query.Select(selector)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector);
        /// <summary>
        /// query.Where(predicate).Select(selector)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector);
        /// <summary>
        /// query.Where(predicate).Skip(page*limit).Take(limit).Select(selector)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, bool>> predicate, int? page, int? limit, Expression<Func<TEntity, TResult>> selector);

        /// <summary>
        /// query.Where(predicate).Select(selector).FirstOrDefault()
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<TResult> SelectFirstAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector);

        /// <summary>
        /// query.Where(predicate).Include(includes).Select(selector).FirstOrDefault
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="selector"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<TResult> SelectFirstAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, params Expression<Func<TEntity, object>>[] includes);
        #endregion

        #endregion
        /// <summary>
        /// query.Any()
        /// </summary>
        /// <returns></returns>
        bool Any();
        /// <summary>
        /// query.Any(predicate)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Any(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// query.Any()
        /// </summary>
        /// <returns></returns>
        Task<bool> AnyAsync();
        /// <summary>
        /// query.Any(predicate)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    }
}
