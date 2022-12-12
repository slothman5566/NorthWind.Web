using AutoMapper;
using Northwind.Data.Models;
using Northwind.Repo;
using Northwind.Service.IService;
using Northwind.Utility.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Service
{
    public class UserService : EntityService<LocalUser>, IUserService
    {
        private readonly IMapper _Mapper;

        public UserService(IUnitOfWork uow,IMapper mapper) : base(uow)
        {
            _Mapper = mapper;
        }

        public bool IsUniqueUser(string userName)
        {
            var user =_UnitOfWork.UserRepository.FindBy(x => x.UserName == userName).FirstOrDefault();
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public Task<LoginRequestViewModel> Login(LoginRequestViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task<UserViewModel> Regiester(RegisterationUserViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
