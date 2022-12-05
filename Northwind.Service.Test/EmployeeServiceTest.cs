using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Northwind.Repo;
using Northwind.Service;
using Northwind.Data;
using Northwind.Data.Models;
using NUnit.Framework;
using System.Linq;
using System.Linq.Expressions;

namespace Northwind.Service.Test
{
    [TestFixture]
    public class EmployeeServiceTest
    {

        private IEmployeeService _employeeService;
        private List<Employee> _employees;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IEmployeeRepository> _employeeRepository;


        [SetUp]
        public void Setup()
        {

            _unitOfWork = new Mock<IUnitOfWork>();
            _employeeRepository = new Mock<IEmployeeRepository>();
            _unitOfWork.Setup(x => x.EmployeeRepository).Returns(_employeeRepository.Object);
            _employeeService = new EmployeeService(_unitOfWork.Object);
            _employees = new List<Employee>() {
                new Employee() { EmployeeId = 1 },
                new Employee() { EmployeeId = 2 },
            new Employee() { EmployeeId = 3}};

            _employeeRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Employee, bool>>>()))
             .Returns((Expression<Func<Employee, bool>> filter) => _employees.Where(filter.Compile()).AsQueryable());
            _employeeRepository.Setup(x => x.GetAll()).Returns(_employees.AsQueryable());

            _employeeRepository.Setup(x => x.Delete(It.IsAny<Employee>())).Returns((Employee employee) =>
            {
                if (_employees.Where(x => x.EmployeeId == employee.EmployeeId).Any())
                {
                    _employees.Remove(_employees.Where(x => x.EmployeeId == employee.EmployeeId).Single());
                    return employee;
                }
                return null;
            });

            _employeeRepository.Setup(x => x.Insert(It.IsAny<Employee>())).Returns((Employee employee) =>
            {
                if (_employees.Where(x => x.EmployeeId == employee.EmployeeId).Any())
                {
                    throw new InvalidOperationException("DuplicateKey");
                }
                _employees.Add(employee);
                return employee;
            });

        }

        [Test]
        public void TestGetAll()
        {


            

            var result = _employeeService.GetAll().ToList();
            Assert.That(result.Count, Is.EqualTo(3));

        }

        [Test]
        public void TestFindBy()
        {


          
            var id = 1;


            var result = _employeeService.FindBy(x => x.EmployeeId == id).First();
            Assert.That(result, Is.EqualTo(_employees.First()));

        }

        [Test]
        public void TestDelete()
        {


        

            var answer = _employees.First();
            var result = _employeeService.Delete(_employees.First());
            Assert.That(result, Is.EqualTo(answer));


            var count = _employeeService.GetAll().ToList();
            Assert.That(count.Count, Is.EqualTo(2));
        }
        [Test]
        public void TestDeleteNotExist()
        {


            var answer = _employees.First();
            var result = _employeeService.Delete(new Employee() { EmployeeId = 4 });
            Assert.That(result, Is.EqualTo(null));


            var count = _employeeService.GetAll().ToList();
            Assert.That(count.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestInsertExistID()
        {
            var exception = Assert.Throws<InvalidOperationException>(()=>_employeeService.Insert(new Employee() { EmployeeId = 1 }));
            Assert.That( exception.Message,Is.EqualTo("DuplicateKey"));
          

        }

        [Test]
        public void TestInsert()
        {
            var result =_employeeService.Insert(new Employee() { EmployeeId = 4 });
            Assert.That(result.EmployeeId, Is.EqualTo(4));
            var count = _employeeService.GetAll().ToList();
            Assert.That(count.Count, Is.EqualTo(4));

        }

    }
}