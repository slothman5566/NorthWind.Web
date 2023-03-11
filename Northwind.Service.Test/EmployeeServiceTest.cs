using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Northwind.Repo;
using Northwind.Data;
using Northwind.Data.Models;
using NUnit.Framework;
using System.Linq;
using System.Linq.Expressions;
using Northwind.Repo.Repo.IRepo;
using Northwind.Service.IService;

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
                new Employee() { EmployeeId = 1,City="AAA" },
                new Employee() { EmployeeId = 2 },
                new Employee() { EmployeeId = 3}};
            _employees.First().ReportsTo = 3;
            _employees.First().ReportsToNavigation = _employees.Last();
            _employeeRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Employee, bool>>>()))
             .Returns((Expression<Func<Employee, bool>> filter) => _employees.Where(filter.Compile()).AsQueryable());
            _employeeRepository.Setup(x => x.GetAll()).Returns(_employees.AsQueryable());

            _employeeRepository.Setup(x => x.Delete(It.IsAny<Employee>())).Callback((Employee employee) =>
            {
                if (_employees.Where(x => x.EmployeeId == employee.EmployeeId).Any())
                {
                    if (_employees.Remove(_employees.Where(x => x.EmployeeId == employee.EmployeeId).Single()))
                    {

                    }

                }

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

            _employeeRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Employee>>())).Callback((IEnumerable<Employee> employees) =>
            {

                foreach (var id in _employees.Join(employees, x => x.EmployeeId, y => y.EmployeeId, (x, y) => x.EmployeeId).ToList())
                {
                    _employees.Remove(_employees.Single(x => x.EmployeeId == id));

                }

            });

            _employeeRepository.Setup(x => x.Edit(It.IsAny<Employee>())).Returns((Employee employee) =>
            {
                if (_employees.Where(x => x.EmployeeId == employee.EmployeeId).Any())
                {
                    var target = _employees.Single(x => x.EmployeeId == employee.EmployeeId);
                    //No
                    target = employee;
                    return target;
                }

                return null;
            });

            _employeeRepository.Setup(x => x.GetAllLazyLoad(It.IsAny<Expression<Func<Employee, bool>>>(), It.IsAny<Expression<Func<Employee, object>>[]>()))
                .Returns((Expression<Func<Employee, bool>> filter, Expression<Func<Employee, object>>[] children) =>
                {
                    if (children != null)
                    {
                        children.ToList().ForEach(_ => _employees.AsQueryable().Include(_).Load());
                    }

                    if (filter == null)
                    {
                        return _employees.AsQueryable();
                    }

                    return _employees.AsQueryable().Where(filter);

                }


                );

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
        public void TestDeleteNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _employeeService.Delete(null));
            Assert.That(exception.ParamName, Is.EqualTo("entity"));


        }

        [Test]
        public void TestDelete()
        {
            var answer = _employees.First();
            _employeeService.Delete(_employees.First());
            var count = _employeeService.GetAll().ToList();

            Assert.That(count.Count, Is.EqualTo(2));

        }
        [Test]
        public void TestDeleteNotExist()
        {


            var answer = _employees.First();
            _employeeService.Delete(new Employee() { EmployeeId = 4 });


            var count = _employeeService.GetAll().ToList();
            Assert.That(count.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestDeleteRange()
        {
            _employeeService.DeleteRange(new List<Employee>() {
                new Employee() { EmployeeId=1},
                new Employee() { EmployeeId=2},
                new Employee() { EmployeeId=3}

            });

            var count = _employeeService.GetAll().ToList();
            Assert.That(count.Count, Is.EqualTo(0));

        }

        [Test]
        public void TestInsertExistID()
        {
            var exception = Assert.Throws<InvalidOperationException>(() => _employeeService.Insert(new Employee() { EmployeeId = 1 }));
            Assert.That(exception.Message, Is.EqualTo("DuplicateKey"));


        }

        [Test]
        public void TestInsert()
        {

            var result = _employeeService.Insert(new Employee() { EmployeeId = 4 });
            Assert.That(result.EmployeeId, Is.EqualTo(4));
            var count = _employeeService.GetAll().ToList();
            Assert.That(count.Count, Is.EqualTo(4));

        }


        [Test]
        public void TestEdit()
        {
            var source = new Employee() { EmployeeId = 1, City = "BBB" };
            var result = _employeeService.Edit(source);
            Assert.That(result.EmployeeId, Is.EqualTo(1));

            Assert.That(result.City, Is.EqualTo("BBB"));

        }



        [Test]
        public async Task TestInsertAsync()
        {
            var result = await _employeeService.InsertAsync(new Employee() { EmployeeId = 4 });
            Assert.That(result.EmployeeId, Is.EqualTo(4));
            var count = _employeeService.GetAll().ToList();
            Assert.That(count.Count, Is.EqualTo(4));

        }

        [Test]
        public async Task TestDeleteAsync()
        {

            var answer = _employees.First();
            await _employeeService.DeleteAsync(_employees.First());


            var count = _employeeService.GetAll().ToList();
            Assert.That(count.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task TestDeleteRangeAsync()
        {
            await _employeeService.DeleteRangeAsync(new List<Employee>() {
                new Employee() { EmployeeId=1},
                new Employee() { EmployeeId=2},
                new Employee() { EmployeeId=3}

            });

            var count = _employeeService.GetAll().ToList();
            Assert.That(count.Count, Is.EqualTo(0));

        }

        [Test]
        public async Task TestEditAsync()
        {
            var source = new Employee() { EmployeeId = 1, City = "BBB" };
            var result = await _employeeService.EditAsync(source);
            Assert.That(result.EmployeeId, Is.EqualTo(1));

            Assert.That(result.City, Is.EqualTo("BBB"));

        }
    }
}