using Northwind.Data;
using Northwind.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Repo.Repo.IRepo
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
    }
}
