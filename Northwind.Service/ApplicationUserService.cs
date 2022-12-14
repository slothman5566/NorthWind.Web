using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Northwind.Data.Models;
using Northwind.Repo;
using Northwind.Service.IService;
using Northwind.Utility;
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
    public class ApplicationUserService : EntityService<ApplicationUser>, IUserService
    {
        private readonly IMapper _Mapper;

        private readonly IConfiguration _Configuration;
        private readonly string _SecretKey;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        public ApplicationUserService(IUnitOfWork uow, IMapper mapper, IConfiguration configuration
            , UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(uow)
        {
            _Mapper = mapper;
            _UserManager = userManager;
            _RoleManager = roleManager;
            _SecretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
        }

        public bool IsUniqueUser(string userName)
        {
            var user = _UnitOfWork.ApplicationUserRepository.FindBy(x => x.UserName == userName).FirstOrDefault();
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseViewModel> Login(LoginRequestViewModel viewModel)
        {
            var user = _UnitOfWork.ApplicationUserRepository.FindBy(u => u.UserName.ToLower() == viewModel.UserName.ToLower() ).FirstOrDefault();
            var isVaild = await _UserManager.CheckPasswordAsync(user, viewModel.Password);
            if (user == null || !isVaild)
            {
                return new LoginResponseViewModel()
                {
                    Token = string.Empty,
                    User = null
                };
            }
            var roles=await _UserManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_SecretKey);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);

            var loginResponse = new LoginResponseViewModel()
            {
                Token = tokenHandler.WriteToken(token),
                User = _Mapper.Map<UserViewModel>(user),
                Role=roles.FirstOrDefault()
            };

            return loginResponse;
        }

        public async Task<UserViewModel> Register(RegisterationUserViewModel viewModel)
        {
            var user = new ApplicationUser()
            {

                UserName = viewModel.UserName,
               
                Name = viewModel.Name,
                
            };

            try
            {
                var result = await _UserManager.CreateAsync(user, viewModel.Password);
                if(result.Succeeded) {

                    if (! await _RoleManager.RoleExistsAsync(SD.Admin))
                    {
                        await _RoleManager.CreateAsync(new IdentityRole(SD.Admin));
                    }
                    if (!await _RoleManager.RoleExistsAsync(SD.Customer))
                    {
                        await _RoleManager.CreateAsync(new IdentityRole(SD.Customer));
                    }
                    await _UserManager.AddToRoleAsync(user,viewModel.Role);
                    var userToReturn=_UnitOfWork.ApplicationUserRepository.FindBy(x=>x.UserName== viewModel.UserName).FirstOrDefault();
                    return _Mapper.Map<UserViewModel>(userToReturn);
                }
            }
            catch(Exception ex)
            {

            }

            return new UserViewModel();
        }
    }
}
