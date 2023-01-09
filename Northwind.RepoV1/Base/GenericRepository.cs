using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.RepoV1.Base
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly NorthwindContext _db;

        public GenericRepository(NorthwindContext context)
        {
            _db = context;
        }

        public Expression<Func<TEntity, bool>> GetPredicate()
        {
            return entity => true;
        }

        #region Create 

        public void Create(TEntity entity)
        {
            if (entity == null)
                return;

            _db.Set<TEntity>().Add(entity);


        }

        public async Task CreateAsync(TEntity entity)
        {
            if (entity == null)
                return;
            await _db.Set<TEntity>().AddAsync(entity);


        }

        public void CreateRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                return;

            _db.Set<TEntity>().AddRange(entities);


        }

        public async Task CreateRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any())
                return;

            await _db.Set<TEntity>().AddRangeAsync(entities);

        }

        #endregion

        #region Delete

        public void Delete(TEntity entity)
        {
            if (entity == null)
                return;
            _db.Set<TEntity>().Remove(entity);
        }


        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any())
                return;
            _db.Set<TEntity>().RemoveRange(entities);

        }

        public void DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            _db.Set<TEntity>().RemoveRange(_db.Set<TEntity>().Where(predicate));
        }

        #endregion

        #region Update 

        public void Update(TEntity entity)
        {
            if (entity == null)
                return;

            _db.Set<TEntity>().Update(entity);

        }

        #endregion

        #region Getter

        #region GetAll
        public IEnumerable<TEntity> GetAll()
        {
            return _db.Set<TEntity>().AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().AsNoTracking().Where(predicate).ToList();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _db.Set<TEntity>().Where(predicate);
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return query.AsNoTracking().ToList();
        }


        public PageResultDto<TEntity> GetAll(int? page, int? limit)
        {
            var query = _db.Set<TEntity>().AsQueryable();
            var count = query.Count();
            if (limit == null)
            {
                limit ??= count;
            }
            page ??= 1;
            var offset = limit * (page - 1);

            return new()
            {
                Page = (int)page,
                Count = count,
                Limit = (int)limit,
                Results = query.AsNoTracking().Skip((int)offset).Take((int)limit).ToList()
            };

        }

        public PageResultDto<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, int? page, int? limit)
        {
            var query = _db.Set<TEntity>().Where(predicate);
            var count = query.Count();
            if (limit == null)
            {
                limit ??= count;
            }
            page ??= 1;
            var offset = limit * (page - 1);
            return new()
            {
                Page = (int)page,
                Count = count,
                Limit = (int)limit,
                Results = query.AsNoTracking().Skip((int)offset).Take((int)limit).ToList()
            };
        }

        public PageResultDto<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, int? page, int? limit, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _db.Set<TEntity>().Where(predicate);
            var count = query.Count();
            if (limit == null)
            {
                limit ??= count;
            }
            page ??= 1;
            var offset = limit * (page - 1);

            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return new()
            {
                Page = (int)page,
                Count = count,
                Limit = (int)limit,
                Results = query.AsNoTracking().Skip((int)offset).Take((int)limit).ToList()
            };
        }

        #endregion

        #region GetAllAsync

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _db.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _db.Set<TEntity>().Where(predicate);
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<PageResultDto<TEntity>> GetAllAsync(int? page, int? limit)
        {
            var query = _db.Set<TEntity>().AsQueryable();
            var count = await query.CountAsync();
            if (limit == null)
            {
                limit ??= count;
            }
            page ??= 1;
            var offset = limit * (page - 1);

            return new()
            {
                Page = (int)page,
                Count = count,
                Limit = (int)limit,
                Results = await query.AsNoTracking().Skip((int)offset).Take((int)limit).ToListAsync()
            };
        }

        public async Task<PageResultDto<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, int? page, int? limit)
        {
            var query = _db.Set<TEntity>().Where(predicate);
            var count = await query.CountAsync();
            if (limit == null)
            {
                limit ??= count;
            }
            page ??= 1;
            var offset = limit * (page - 1);

            return new()
            {
                Page = (int)page,
                Count = count,
                Limit = (int)limit,
                Results = await query.AsNoTracking().Skip((int)offset).Take((int)limit).ToListAsync()
            };
        }

        public async Task<PageResultDto<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, int? page, int? limit, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _db.Set<TEntity>().Where(predicate);
            var count = await query.CountAsync();
            if (limit == null)
            {
                limit ??= count;
            }
            page ??= 1;
            var offset = limit * (page - 1);
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }
            return new()
            {
                Page = (int)page,
                Count = count,
                Limit = (int)limit,
                Results = await query.AsNoTracking().Skip((int)offset).Take((int)limit).ToListAsync()
            };
        }


        #endregion

        #region GetFirst
        public TEntity GetFirst()
        {
            return _db.Set<TEntity>().AsNoTracking().FirstOrDefault();
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().AsNoTracking().FirstOrDefault(predicate);
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _db.Set<TEntity>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }
            return query.AsNoTracking().FirstOrDefault(predicate);
        }
        #endregion

        #region GetFirstAsync

        public async Task<TEntity> GetFirstAsync()
        {
            return await _db.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _db.Set<TEntity>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        #endregion

        #region GetLast

        public TEntity GetLast()
        {
            return _db.Set<TEntity>().AsNoTracking().LastOrDefault();
        }

        public TEntity GetLast(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().AsNoTracking().LastOrDefault(predicate);
        }

        public TEntity GetLast(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _db.Set<TEntity>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }
            return query.AsNoTracking().LastOrDefault(predicate);

        }

        #endregion

        #region GetLastAsync

        public async Task<TEntity> GetLastAsync()
        {
            return await _db.Set<TEntity>().AsNoTracking().LastOrDefaultAsync();
        }

        public async Task<TEntity> GetLastAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Set<TEntity>().AsNoTracking().LastOrDefaultAsync(predicate);
        }

        public async Task<TEntity> GetLastAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _db.Set<TEntity>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }
            return await query.AsNoTracking().LastOrDefaultAsync(predicate);
        }

        #endregion

        #endregion

        #region Select
        #region Select

        public IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return _db.Set<TEntity>().AsNoTracking().Select(selector).ToList();
        }

        public IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
        {

            return _db.Set<TEntity>().AsNoTracking().Where(predicate).Select(selector).ToList();

        }

        public PageResultDto<TResult> Select<TResult>(Expression<Func<TEntity, bool>> predicate, int? page, int? limit, Expression<Func<TEntity, TResult>> selector)
        {
            var query = _db.Set<TEntity>().Where(predicate);
            var count = query.Count();
            if (limit == null)
            {
                limit ??= count;
            }
            page ??= 1;
            var offset = limit * (page - 1);
            return new()
            {
                Page = (int)page,
                Count = count,
                Limit = (int)limit,
                Results = query.AsNoTracking().Skip((int)offset).Take((int)limit).Select(selector).ToList()
            };
        }

        #endregion

        #region SelectAsync

        public async Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return await _db.Set<TEntity>().AsNoTracking().Select(selector).ToListAsync();
        }

        public async Task<IEnumerable<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
        {
            return await _db.Set<TEntity>().AsNoTracking().Where(predicate).Select(selector).ToListAsync();
        }

        public async Task<PageResultDto<TResult>> SelectAsync<TResult>(Expression<Func<TEntity, bool>> predicate, int? page, int? limit, Expression<Func<TEntity, TResult>> selector)
        {
            var query = _db.Set<TEntity>().Where(predicate);
            var count = await query.CountAsync();
            if (limit == null)
            {
                limit ??= count;
            }
            page ??= 1;
            var offset = limit * (page - 1);
            return new()
            {
                Page = (int)page,
                Count = count,
                Limit = (int)limit,
                Results = await query.AsNoTracking().Skip((int)offset).Take((int)limit).Select(selector).ToListAsync()
            };
        }

        public async Task<TResult> SelectFirstAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
        {

            return await _db.Set<TEntity>().AsNoTracking().Where(predicate).Select(selector).FirstOrDefaultAsync();
        }

        public async Task<TResult> SelectFirstAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _db.Set<TEntity>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }
            return await query.AsNoTracking().Where(predicate).Select(selector).FirstOrDefaultAsync();
        }

        #endregion

        #endregion

        #region Any

        public bool Any()
        {
            return _db.Set<TEntity>().AsNoTracking().Any();
        }

        public async Task<bool> AnyAsync()
        {
            return await _db.Set<TEntity>().AsNoTracking().AnyAsync();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().AsNoTracking().Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Set<TEntity>().AsNoTracking().AnyAsync(predicate);
        }


        #endregion

        #region Count
        public int Count()
        {
            return _db.Set<TEntity>().AsNoTracking().Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().AsNoTracking().Count(predicate);
        }

        public async Task<int> CountAsync()
        {
            return await _db.Set<TEntity>().AsNoTracking().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Set<TEntity>().AsNoTracking().CountAsync(predicate);
        }

        #endregion

    }
}
