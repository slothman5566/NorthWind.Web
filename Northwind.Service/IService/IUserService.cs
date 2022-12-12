using Northwind.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Northwind.Utility.Model.ViewModel;

namespace Northwind.Service.IService
{
    public interface IUserService : IEntityService<LocalUser>
    {
        bool IsUniqueUser(string userName);
        
        Task<LoginResponseViewModel> Login(LoginRequestViewModel viewModel);

        Task<UserViewModel> Register(RegisterationUserViewModel viewModel);

    }
}
