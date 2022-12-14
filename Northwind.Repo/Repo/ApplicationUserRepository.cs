using Microsoft.EntityFrameworkCore;
using Northwind.Data.Models;
using Northwind.Repo.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Repo.Repo
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
