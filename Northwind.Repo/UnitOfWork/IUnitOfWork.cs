using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Repo.Repo.IRepo;

namespace Northwind.Repo
{
    public interface IUnitOfWork
    {
        IEmployeeRepository EmployeeRepository { get; }
       
        IUserRepository UserRepository { get; }
        void SaveChange();

         Task<int> SaveChangeAsync();
    }
}
