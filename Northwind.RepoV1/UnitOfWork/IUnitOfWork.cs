using Northwind.RepoV1.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.RepoV1.UnitOfWork
{
    public interface IUnitOfWork
    {
        IEmployeeRepository EmployeeRepository { get; }

     
        void SaveChange();

        Task<int> SaveChangeAsync();
    }
}
