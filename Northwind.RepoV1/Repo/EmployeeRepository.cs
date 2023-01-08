using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Data.Models;
using Northwind.RepoV1.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.RepoV1.Repo
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(NorthwindContext dbContext) : base(dbContext)
        {
        }
    }
}
