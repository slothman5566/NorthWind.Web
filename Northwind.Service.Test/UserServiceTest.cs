using Moq;
using Northwind.Data.Models;
using Northwind.Repo.Repo.IRepo;
using Northwind.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Northwind.Service.IService;
using Northwind.Utility.Model.ViewModel;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Mapster;
using MapsterMapper;

namespace Northwind.Service.Test
{

    [TestFixture]
    public class UserServiceTest
    {
        protected IUserService _userService;
        protected List<LocalUser> _users;
        protected Mock<IUnitOfWork> _unitOfWork;
        protected Mock<IUserRepository> _userRepository;


        [SetUp]
        public void Setup()
        {

            _unitOfWork = new Mock<IUnitOfWork>();
            _userRepository = new Mock<IUserRepository>();
            _unitOfWork.Setup(x => x.UserRepository).Returns(_userRepository.Object);
            var myConfiguration = new Dictionary<string, string>
            {
                 {"ApiSettings:SecretKey", "this is my custom Secret key for authentication"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
            _userService = new UserService(_unitOfWork.Object, new Mapper(), configuration);
            _users = new List<LocalUser>() {
                new LocalUser() {Id=1,UserName="AAA",Password="AAA",Name="AAA",Role="Admin"},
                new LocalUser() {Id=2,UserName="BBB",Password="BBB",Name="BBB" },
                new LocalUser() {Id=3,UserName="CCC",Password="CCC", Name = "CCC"}};



            _userRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<LocalUser, bool>>>()))
             .Returns((Expression<Func<LocalUser, bool>> filter) => _users.Where(filter.Compile()).AsQueryable());
            _userRepository.Setup(x => x.GetAll()).Returns(

                _users.AsQueryable()
                );

            _userRepository.Setup(x => x.Delete(It.IsAny<LocalUser>())).Callback((LocalUser user) =>
            {
                if (_users.Where(x => x.Id == user.Id).Any())
                {
                    if (_users.Remove(_users.Where(x => x.Id == user.Id).Single()))
                    {
                        
                    }

                }
                
            });


            _userRepository.Setup(x => x.Insert(It.IsAny<LocalUser>())).Returns((LocalUser user) =>
            {
                if (_users.Where(x => x.Id == user.Id).Any())
                {
                    throw new InvalidOperationException("DuplicateKey");
                }
                _users.Add(user);
                return user;
            });

            _userRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<LocalUser>>())).Callback((IEnumerable<LocalUser> users) =>
            {

                foreach (var id in _users.Join(users, x => x.Id, y => y.Id, (x, y) => x.Id).ToList())
                {
                    _users.Remove(_users.Single(x => x.Id == id));

                }

            });

            _userRepository.Setup(x => x.Edit(It.IsAny<LocalUser>())).Returns((LocalUser user) =>
            {
                if (_users.Where(x => x.Id == user.Id).Any())
                {
                    var target = _users.Single(x => x.Id == user.Id);
                    //No
                    target = user;
                    return target;
                }

                return null;
            });


        }

        [Test]
        [TestCase("AAA", ExpectedResult = false)]
        [TestCase("AAAC", ExpectedResult = true)]

        public bool TestIsUniqueName(string name)
        {

            return _userService.IsUniqueUser(name);
        }

        [Test]
        public virtual  async Task TestRegiester()
        {

            var user = await _userService.Register(new RegisterationUserViewModel()
            {
                Name = "DDD",
                Password = "DDD",

            });
            Assert.IsNotNull(user);

            Assert.That(_users.Count, Is.EqualTo(4));

        }

        [Test]

        public virtual async Task TestLoginSucess()
        {
            var loginRequest = new LoginRequestViewModel() { UserName = "AAA", Password = "AAA" };
            var loginResponse = await _userService.Login(loginRequest);

            Assert.IsNotNull(loginResponse);
            Assert.That(loginResponse.Token, Is.Not.EqualTo(string.Empty));
        }

        [Test]

        public virtual async Task TestLoginFailed()
        {
            var loginRequest = new LoginRequestViewModel() { UserName = "AAAC", Password = "AAA" };
            var loginResponse = await _userService.Login(loginRequest);

            Assert.IsNotNull(loginResponse);
            Assert.That(loginResponse.Token, Is.EqualTo(string.Empty));
        }

    }
}
