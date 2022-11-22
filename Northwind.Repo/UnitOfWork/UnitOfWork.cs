using Microsoft.EntityFrameworkCore;
using NorthWind.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        protected NorthwindContext _DbContext;
        protected IEmployeeRepository _EmployeeRepository;

        protected bool _Disposed;

        public IEmployeeRepository EmployeeRepository => _EmployeeRepository ?? (_EmployeeRepository = new EmployeeRepository(_DbContext));
        public UnitOfWork(NorthwindContext dbContext)
        {
            _DbContext = dbContext;
        }
        public void Complete()
        {

            _DbContext.SaveChanges();


        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    _DbContext.Dispose();
                    _DbContext = null;
                }
                _Disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
