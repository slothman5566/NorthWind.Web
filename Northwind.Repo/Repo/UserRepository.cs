using Microsoft.EntityFrameworkCore;
using Northwind.Data.Models;
using Northwind.Repo.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Repo
{
    public class UserRepository : GenericRepository<LocalUser>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
