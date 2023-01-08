using Northwind.Repo;
using Northwind.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Service.IService;

namespace Northwind.Service
{
    public class EmployeeService : EntityService<Employee>, IEmployeeService
    {
        public EmployeeService(IUnitOfWork uow) : base(uow,uow.EmployeeRepository)
        {

        }
    }
}
