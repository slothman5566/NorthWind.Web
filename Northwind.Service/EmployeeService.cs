using Northwind.Repo;
using NorthWind.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Service
{
    public class EmployeeService : EntityService<Employee>, IEmployeeService
    {
        public EmployeeService(IUnitOfWork uow) : base(uow)
        {
            _Repository = _UnitOfWork.EmployeeRepository;
        }
    }
}
