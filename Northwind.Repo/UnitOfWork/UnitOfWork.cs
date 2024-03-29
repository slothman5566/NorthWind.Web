﻿using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Repo.Repo;
using Northwind.Repo.Repo.IRepo;
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
        protected IUserRepository _UserRepository;

        protected IApplicationUserRepository _ApplicationUserRepository;
        protected bool _Disposed;

        public IEmployeeRepository EmployeeRepository => _EmployeeRepository ?? (_EmployeeRepository = new EmployeeRepository(_DbContext));
        public IUserRepository UserRepository => _UserRepository ?? (_UserRepository = new UserRepository(_DbContext));
        public IApplicationUserRepository ApplicationUserRepository => _ApplicationUserRepository ?? (_ApplicationUserRepository = new ApplicationUserRepository(_DbContext));



        public UnitOfWork(NorthwindContext dbContext)
        {
            _DbContext = dbContext;
        }
        public void SaveChange()
        {

            _DbContext.SaveChanges();


        }
        public async Task<int> SaveChangeAsync()
        {
          return await  _DbContext.SaveChangesAsync();
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
