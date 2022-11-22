using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Repo
{
    public interface IUnitOfWork
    {
        IEmployeeRepository EmployeeRepository { get; }
       
        void Complete();
    }
}
