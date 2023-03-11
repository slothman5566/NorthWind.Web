using FluentAssertions;
using Northwind.Utility.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Service.Test.FluentAssertions
{
    public class FluentUserServiceTest: UserServiceTest
    {
        [Test]
        public override async Task TestRegiester()
        {

            var user = await _userService.Register(new RegisterationUserViewModel()
            {
                Name = "DDD",
                Password = "DDD",

            });
            user.Should().NotBeNull();
            _users.Count.Should().Be(4);

        }

        [Test]

        public override async Task TestLoginSucess()
        {
            var loginRequest = new LoginRequestViewModel() { UserName = "AAA", Password = "AAA" };
            var loginResponse = await _userService.Login(loginRequest);
            loginResponse.Should().NotBeNull();
            loginResponse.Token.Should().NotBeNullOrEmpty();
           
        }

        [Test]

        public override async Task TestLoginFailed()
        {
            var loginRequest = new LoginRequestViewModel() { UserName = "AAAC", Password = "AAA" };
            var loginResponse = await _userService.Login(loginRequest);

            loginResponse.Should().NotBeNull();
            loginResponse.Token.Should().BeNullOrEmpty();
        }

    }
}
