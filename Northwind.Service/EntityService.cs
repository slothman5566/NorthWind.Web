using Northwind.Repo;
using NorthWind.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Service
{

    public interface IEntityService<TEntity> where TEntity : class
    {


        IEnumerable<TEntity> GetAll();
    }

    public abstract class EntityService<TEntity> : IEntityService<TEntity> where TEntity : class
    {
        protected IUnitOfWork _UnitOfWork;
        protected IGenericRepository<TEntity> _Repository;
        public EntityService(IUnitOfWork uow)
        {
            _UnitOfWork = uow;

        }
        public IEnumerable<TEntity> GetAll()
        {
            return _Repository.GetAll();
        }


    }
}
