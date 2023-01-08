using Northwind.Data;
using Northwind.RepoV1.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.RepoV1.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        protected NorthwindContext _DbContext;
        protected IEmployeeRepository _EmployeeRepository;
        public IEmployeeRepository EmployeeRepository => _EmployeeRepository ?? (_EmployeeRepository = new EmployeeRepository(_DbContext));


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
            return await _DbContext.SaveChangesAsync();
        }

    }
}
