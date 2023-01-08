using Northwind.Data.Models;
using Northwind.RepoV1.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.RepoV1.Repo
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
    }
}
