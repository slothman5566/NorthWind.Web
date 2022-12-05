using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Repo
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
