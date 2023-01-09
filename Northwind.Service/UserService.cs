using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Northwind.Data.Models;
using Northwind.Repo;
using Northwind.Service.IService;
using Northwind.Utility.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Service
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMapper _Mapper;

        private readonly IConfiguration _Configuration;
        private readonly string secretKey;

        public UserService(IUnitOfWork uow, IMapper mapper, IConfiguration configuration) : base(uow)
        {
            _Mapper = mapper;
            secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
        }

        public bool IsUniqueUser(string userName)
        {
            var user = _UnitOfWork.UserRepository.FindBy(x => x.UserName == userName).FirstOrDefault();
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseViewModel> Login(LoginRequestViewModel viewModel)
        {
            var user = _UnitOfWork.UserRepository.FindBy(u => u.UserName.ToLower() == viewModel.UserName.ToLower() &&
            u.Password == viewModel.Password).FirstOrDefault();
            if (user == null)
            {
                return new LoginResponseViewModel()
                {
                    Token = string.Empty,
                    User = null
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);

            var loginResponse = new LoginResponseViewModel()
            {
                Token = tokenHandler.WriteToken(token),
                User = _Mapper.Map<UserViewModel>(user),
                Role = user.Role
            };

            return loginResponse;
        }

        public async Task<UserViewModel> Register(RegisterationUserViewModel viewModel)
        {
            var user = new LocalUser()
            {

                UserName = viewModel.UserName,
                Password = viewModel.Password,
                Name = viewModel.Name,
                Role = viewModel.Role
            };

            _UnitOfWork.UserRepository.Insert(user);
            await _UnitOfWork.SaveChangeAsync();

            return _Mapper.Map<UserViewModel>(user);
        }
    }
}
