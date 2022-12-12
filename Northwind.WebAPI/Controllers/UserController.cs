using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Service.IService;
using Northwind.Utility.Model;
using Northwind.Utility.Model.ViewModel;
using System.Net;

namespace Northwind.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        protected IUserService _UserService;
        public UserController(IUserService userService) {
            _UserService=userService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestViewModel model) {

            var response = new APIResponse();
            var loginResponse = await _UserService.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(response);
            }
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            response.Result = loginResponse;
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationUserViewModel model)
        {
            var response = new APIResponse();
            bool ifUserNameUnique = _UserService.IsUniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Username already exists");
                return BadRequest(response);
            }

            var user = await _UserService.Register(model);
            if (user == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Error while registering");
                return BadRequest(response);
            }
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            return Ok(response);
        }

    }
}
