using Microsoft.EntityFrameworkCore;
using Moq;
using Northwind.Data;
using Northwind.Data.Models;
using Northwind.RepoV1.UnitOfWork;
using NUnit.Framework;
using System.Reflection.Metadata;

namespace Northwind.RepoV1.Test
{
    public class Tests
    {
        private DbContextOptions<NorthwindContext> _options;
        private List<Employee> _employees;
        private IUnitOfWork _unitOfWork;
        private NorthwindContext _dbContext;
        [SetUp]
        public void Setup()
        {
            _employees = new List<Employee>()
            {
                new Employee()
                {
                    EmployeeId= 1,
                    FirstName="A",
                    LastName="A",

                },
                new Employee()
                {
                    EmployeeId= 2,
                    FirstName="A",
                    LastName="B"
                },
                new Employee()
                {
                    EmployeeId= 3,
                    FirstName="A",
                    LastName="C"
                }


            };
            _employees[0].ReportsTo = 2;
            _employees[0].ReportsToNavigation = _employees[1];

            _employees[1].ReportsTo = 3;
            _employees[1].ReportsToNavigation = _employees[2];

            _employees[2].ReportsTo = 1;
            _employees[2].ReportsToNavigation = _employees[0];

            var options = new DbContextOptionsBuilder<NorthwindContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;
            _dbContext = new NorthwindContext(options);
            _dbContext.Database.EnsureCreated();
            if (_dbContext.Users.Count() <= 0)
            {
                _dbContext.Employees.AddRange(_employees);
                _dbContext.SaveChanges();
            }

            _unitOfWork = new UnitOfWork.UnitOfWork(_dbContext);
        }

        #region Create

        private Employee Create()
        {
            var employee = new Employee()
            {

                FirstName = "A",
                LastName = "D",
            };
            employee.ReportsTo = 1;
            employee.ReportsToNavigation = _employees[0];
            return employee;
        }

        private IEnumerable<Employee> CreateRange()
        {
            var employees = new List<Employee>()
            {
                new Employee()
                {

                    FirstName = "A",
                    LastName = "D",
                },new Employee()
                {

                    FirstName = "A",
                    LastName = "E",
                }

            };

            employees.Last().ReportsTo = 1;
            employees.Last().ReportsToNavigation = _employees[0];

            return employees;
        }


        [Test]
        public void TestCreate()
        {
            var employee = Create();

            _unitOfWork.EmployeeRepository.Create(employee);
            _unitOfWork.SaveChange();
            var result = _dbContext.Employees.Include(x => x.ReportsToNavigation).Last();

            Assert.That(result.EmployeeId, Is.EqualTo(employee.EmployeeId));
            Assert.That(result.ReportsToNavigation.EmployeeId, Is.EqualTo(employee.ReportsToNavigation.EmployeeId));

        }

        [Test]
        public async Task TestCreateAsync()
        {
            var employee = Create();

            await _unitOfWork.EmployeeRepository.CreateAsync(employee);
            _unitOfWork.SaveChange();
            var result = _dbContext.Employees.Include(x => x.ReportsToNavigation).Last();

            Assert.That(result.EmployeeId, Is.EqualTo(employee.EmployeeId));
            Assert.That(result.ReportsToNavigation.EmployeeId, Is.EqualTo(employee.ReportsToNavigation.EmployeeId));

        }

        [Test]
        public void TestCreateRange()
        {

            var employees = CreateRange();

            _unitOfWork.EmployeeRepository.CreateRange(employees);

            _unitOfWork.SaveChange();
            Assert.That(_dbContext.Employees.Count(), Is.EqualTo(5));

            var result = _dbContext.Employees.Include(x => x.ReportsToNavigation).OrderBy(x => x.EmployeeId).Last();

            Assert.That(result.EmployeeId, Is.EqualTo(employees.Last().EmployeeId));
            Assert.That(result.ReportsToNavigation.EmployeeId, Is.EqualTo(employees.Last().ReportsToNavigation.EmployeeId));


        }

        [Test]
        public async Task TestCreateRangeAsync()
        {

            var employees = CreateRange();

            await _unitOfWork.EmployeeRepository.CreateRangeAsync(employees);

            _unitOfWork.SaveChange();
            Assert.That(_dbContext.Employees.Count(), Is.EqualTo(5));

            var result = _dbContext.Employees.Include(x => x.ReportsToNavigation).OrderBy(x => x.EmployeeId).Last();

            Assert.That(result.EmployeeId, Is.EqualTo(employees.Last().EmployeeId));
            Assert.That(result.ReportsToNavigation.EmployeeId, Is.EqualTo(employees.Last().ReportsToNavigation.EmployeeId));


        }

        #endregion

        #region Update

        [Test]
        public void TestUpdate()
        {
            _dbContext.ChangeTracker.Clear();
            var first = _unitOfWork.EmployeeRepository.GetFirst();
            first.FirstName= "Test";
            _unitOfWork.EmployeeRepository.Update(first);

            Assert.That(_dbContext.Employees.First().FirstName,Is.EqualTo("Test"));

        }

        #endregion

        #region Delete

        [Test]
        public void TestDelete()
        {
            _unitOfWork.EmployeeRepository.Delete(_employees.Last());
            _unitOfWork.SaveChange();

            Assert.That(_dbContext.Employees.Last().EmployeeId, Is.EqualTo(2));

        }

        [Test]
        public void TestDeleteRange()
        {
            _unitOfWork.EmployeeRepository.DeleteRange(_employees);
            _unitOfWork.SaveChange();

            Assert.That(_dbContext.Employees.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TestDeleteRangeWithPredicate()
        {
            _unitOfWork.EmployeeRepository.DeleteRange(x => x.EmployeeId > 0);
            _unitOfWork.SaveChange();

            Assert.That(_dbContext.Employees.Count(), Is.EqualTo(0));
        }

        #endregion

        #region GetAll

        [Test]
        public void TestGetAll()
        {
            var result = _unitOfWork.EmployeeRepository.GetAll();

            CollectionAssert.AreEqual(result.Select(x => x.EmployeeId), _dbContext.Employees.Select(x => x.EmployeeId));
        }

        [Test]
        public void TestGetAllWithPage()
        {
            var result = _unitOfWork.EmployeeRepository.GetAll(2, 1);

            CollectionAssert.AreEqual(result.Results.Select(x => x.EmployeeId), _dbContext.Employees.Skip(1).Take(1).Select(x => x.EmployeeId));
        }

        [Test]
        public void TestGetAllWithPredicate()
        {
            var result = _unitOfWork.EmployeeRepository.GetAll(x => x.EmployeeId > 1);

            CollectionAssert.AreEqual(result.Select(x => x.EmployeeId), _dbContext.Employees.Where(x => x.EmployeeId > 1).Select(x => x.EmployeeId));
        }

        [Test]
        public void TestGetAllWithPredicatePage()
        {
            var result = _unitOfWork.EmployeeRepository.GetAll(x => x.EmployeeId > 1, 2, 1);

            CollectionAssert.AreEqual(result.Results.Select(x => x.EmployeeId), _dbContext.Employees.Where(x => x.EmployeeId > 1).Skip(1).Take(1).Select(x => x.EmployeeId));
        }

        [Test]
        public void TestGetAllWithPredicateInclude()
        {
            var result = _unitOfWork.EmployeeRepository.GetAll(x => x.EmployeeId > 1, x => x.ReportsToNavigation);

            CollectionAssert.AreEqual(result.Select(x => x.ReportsToNavigation.EmployeeId),
                _dbContext.Employees.Where(x => x.EmployeeId > 1).Include(x => x.ReportsToNavigation).Select(x => x.ReportsToNavigation.EmployeeId));
        }
        [Test]
        public void TestGetAllWithPredicatePageInclude()
        {
            var result = _unitOfWork.EmployeeRepository.GetAll(x => x.EmployeeId > 1, 2, 1, x => x.ReportsToNavigation);
            var anwser = _dbContext.Employees.Where(x => x.EmployeeId > 1).Include(x => x.ReportsToNavigation).Skip(1).Take(1)
                .Select(x => x.ReportsToNavigation.EmployeeId).ToList();
            CollectionAssert.AreEqual(result.Results.Select(x => x.ReportsToNavigation.EmployeeId), anwser
                );
        }

        [Test]
        public async Task TestGetAllAsync()
        {
            var result = await _unitOfWork.EmployeeRepository.GetAllAsync();

            CollectionAssert.AreEqual(result.Select(x => x.EmployeeId), _dbContext.Employees.Select(x => x.EmployeeId));
        }

        [Test]
        public async Task TestGetAllAsyncWithPage()
        {
            var result = await _unitOfWork.EmployeeRepository.GetAllAsync(2, 1);

            CollectionAssert.AreEqual(result.Results.Select(x => x.EmployeeId), _dbContext.Employees.Skip(1).Take(1).Select(x => x.EmployeeId));
        }

        [Test]
        public async Task TestGetAllAsyncWithPredicate()
        {
            var result = await _unitOfWork.EmployeeRepository.GetAllAsync(x => x.EmployeeId > 1);

            CollectionAssert.AreEqual(result.Select(x => x.EmployeeId), _dbContext.Employees.Where(x => x.EmployeeId > 1).Select(x => x.EmployeeId));
        }

        [Test]
        public async Task TestGetAllAsyncWithPredicatePage()
        {
            var result = await _unitOfWork.EmployeeRepository.GetAllAsync(x => x.EmployeeId > 1, 2, 1);

            CollectionAssert.AreEqual(result.Results.Select(x => x.EmployeeId), _dbContext.Employees.Where(x => x.EmployeeId > 1).Skip(1).Take(1).Select(x => x.EmployeeId));
        }

        [Test]
        public async Task TestGetAllAsyncWithPredicateInclude()
        {
            var result = await _unitOfWork.EmployeeRepository.GetAllAsync(x => x.EmployeeId > 1, x => x.ReportsToNavigation);

            CollectionAssert.AreEqual(result.Select(x => x.ReportsToNavigation.EmployeeId),
                _dbContext.Employees.Where(x => x.EmployeeId > 1).Include(x => x.ReportsToNavigation).Select(x => x.ReportsToNavigation.EmployeeId));
        }
        [Test]
        public async Task TestGetAllAsyncWithPredicatePageInclude()
        {
            var result = await _unitOfWork.EmployeeRepository.GetAllAsync(x => x.EmployeeId > 1, 2, 1, x => x.ReportsToNavigation);
            var anwser = await _dbContext.Employees.Where(x => x.EmployeeId > 1).Include(x => x.ReportsToNavigation)
                .Skip(1).Take(1).ToListAsync();
            CollectionAssert.AreEqual(result.Results.Select(x => x.ReportsToNavigation.EmployeeId),
             anwser.Select(x => x.ReportsToNavigation.EmployeeId));
        }

        #endregion

        #region GetFirst
        [Test]
        public void TestGetFirst()
        {
            var result = _unitOfWork.EmployeeRepository.GetFirst();

            Assert.That(result.EmployeeId, Is.EqualTo(_employees.First().EmployeeId));

        }


        [Test]
        public void TestGetFirstWitnPredicate()
        {
            var result = _unitOfWork.EmployeeRepository.GetFirst(x => x.EmployeeId > 1);

            Assert.That(result.EmployeeId, Is.EqualTo(_employees.First(x => x.EmployeeId > 1).EmployeeId));

        }


        [Test]
        public void TestGetFirstWitnPredicateInclude()
        {
            var result = _unitOfWork.EmployeeRepository.GetFirst(x => x.EmployeeId > 1, x => x.ReportsToNavigation);

            Assert.That(result.ReportsToNavigation.EmployeeId, Is.EqualTo(_employees.First(x => x.EmployeeId > 1).ReportsToNavigation.EmployeeId));

        }


        [Test]
        public async Task TestGetFirstAsync()
        {
            var result = await _unitOfWork.EmployeeRepository.GetFirstAsync();

            Assert.That(result.EmployeeId, Is.EqualTo(_employees.First().EmployeeId));

        }


        [Test]
        public async Task TestGetFirstAsyncWitnPredicate()
        {
            var result = await _unitOfWork.EmployeeRepository.GetFirstAsync(x => x.EmployeeId > 1);

            Assert.That(result.EmployeeId, Is.EqualTo(_employees.First(x => x.EmployeeId > 1).EmployeeId));

        }


        [Test]
        public async Task TestGetFirstAsyncWitnPredicateInclude()
        {
            var result = await _unitOfWork.EmployeeRepository.GetFirstAsync(x => x.EmployeeId > 1, x => x.ReportsToNavigation);

            Assert.That(result.ReportsToNavigation.EmployeeId, Is.EqualTo(_employees.First(x => x.EmployeeId > 1).ReportsToNavigation.EmployeeId));

        }

        #endregion

        #region GetLast

        [Test]
        public void TestGetLast()
        {
            var result = _unitOfWork.EmployeeRepository.GetLast();

            Assert.That(result.EmployeeId, Is.EqualTo(_employees.Last().EmployeeId));

        }


        [Test]
        public void TestGetLastWitnPredicate()
        {
            var result = _unitOfWork.EmployeeRepository.GetLast(x => x.EmployeeId > 1);

            Assert.That(result.EmployeeId, Is.EqualTo(_employees.Last(x => x.EmployeeId > 1).EmployeeId));

        }


        [Test]
        public void TestGetLastWitnPredicateInclude()
        {
            var result = _unitOfWork.EmployeeRepository.GetLast(x => x.EmployeeId > 1, x => x.ReportsToNavigation);

            Assert.That(result.ReportsToNavigation.EmployeeId, Is.EqualTo(_employees.Last(x => x.EmployeeId > 1).ReportsToNavigation.EmployeeId));

        }


        [Test]
        public async Task TestGetLastAsync()
        {
            var result = await _unitOfWork.EmployeeRepository.GetLastAsync();

            Assert.That(result.EmployeeId, Is.EqualTo(_employees.Last().EmployeeId));

        }


        [Test]
        public async Task TestGetLastAsyncWitnPredicate()
        {
            var result = await _unitOfWork.EmployeeRepository.GetLastAsync(x => x.EmployeeId > 1);

            Assert.That(result.EmployeeId, Is.EqualTo(_employees.Last(x => x.EmployeeId > 1).EmployeeId));

        }


        [Test]
        public async Task TestGetLastAsyncWitnPredicateInclude()
        {
            var result = await _unitOfWork.EmployeeRepository.GetLastAsync(x => x.EmployeeId > 1, x => x.ReportsToNavigation);

            Assert.That(result.ReportsToNavigation.EmployeeId, Is.EqualTo(_employees.Last(x => x.EmployeeId > 1).ReportsToNavigation.EmployeeId));

        }

        #endregion

        #region Any

        [Test]
        public async Task TestAny()
        {
            Assert.IsTrue(_unitOfWork.EmployeeRepository.Any());
            Assert.IsTrue(_unitOfWork.EmployeeRepository.Any(x => x.EmployeeId > 0));

            Assert.IsTrue(await _unitOfWork.EmployeeRepository.AnyAsync());
            Assert.IsTrue(await _unitOfWork.EmployeeRepository.AnyAsync(x => x.EmployeeId > 0));
        }

        [Test]
        public async Task TestAnyIsFalse()
        {
            Assert.IsFalse(_unitOfWork.EmployeeRepository.Any(x => x.EmployeeId > 3));
            _dbContext.Employees.RemoveRange(_employees);
            _dbContext.SaveChanges();
            Assert.IsFalse(_unitOfWork.EmployeeRepository.Any());
            Assert.IsFalse(_unitOfWork.EmployeeRepository.Any(x => x.EmployeeId > 0));
            Assert.IsFalse(await _unitOfWork.EmployeeRepository.AnyAsync());
            Assert.IsFalse(await _unitOfWork.EmployeeRepository.AnyAsync(x => x.EmployeeId > 0));
        }
        #endregion

        #region Select

        [Test]
        public void TestSelect()
        {
            Assert.That(_unitOfWork.EmployeeRepository.Select(x => x.EmployeeId), Is.EqualTo(Enumerable.Range(1, 3)));

            Assert.That(_unitOfWork.EmployeeRepository.Select(x => x.EmployeeId > 1, x => x.EmployeeId), Is.EqualTo(Enumerable.Range(2, 2)));

            Assert.That(_unitOfWork.EmployeeRepository.Select(x => x.EmployeeId > 0, 2, 2, x => x.EmployeeId).Results, Is.EqualTo(Enumerable.Range(3, 1)));
        }

        [Test]
        public async Task TestSelectAsync()
        {
            Assert.That(await _unitOfWork.EmployeeRepository.SelectAsync(x => x.EmployeeId), Is.EqualTo(Enumerable.Range(1, 3)));

            Assert.That(await _unitOfWork.EmployeeRepository.SelectAsync(x => x.EmployeeId > 1, x => x.EmployeeId), Is.EqualTo(Enumerable.Range(2, 2)));

            Assert.That((await _unitOfWork.EmployeeRepository.SelectAsync(x => x.EmployeeId > 0, 2, 2, x => x.EmployeeId)).Results, Is.EqualTo(Enumerable.Range(3, 1)));
        }

        [Test]
        public async Task TsetSelectFirstAsync()
        {
            var result = await _unitOfWork.EmployeeRepository.SelectFirstAsync(x => x.EmployeeId > 1, x => x);
            Assert.That(result.EmployeeId, Is.EqualTo(2));

            Assert.IsNull(result.ReportsToNavigation);

            var resultInclude = await _unitOfWork.EmployeeRepository.SelectFirstAsync(x => x.EmployeeId > 1, x => x, x => x.ReportsToNavigation);
            Assert.IsNotNull(resultInclude.ReportsToNavigation);
            Assert.That(resultInclude.EmployeeId, Is.EqualTo(_employees[1].EmployeeId));
            Assert.That(resultInclude.ReportsToNavigation.EmployeeId, Is.EqualTo(_employees[2].EmployeeId));

        }

        #endregion

        #region Count

        [Test]
        public void TsetCount()
        {
            Assert.That(_unitOfWork.EmployeeRepository.Count(), Is.EqualTo(_dbContext.Employees.Count()));
        }


        [Test]
        public async Task TsetCountAsync()
        {
            Assert.That(await _unitOfWork.EmployeeRepository.CountAsync(), Is.EqualTo(await _dbContext.Employees.CountAsync()));
        }

        [Test]
        public void TsetCountWithPredicate()
        {
            Assert.That(_unitOfWork.EmployeeRepository.Count(x => x.EmployeeId > 1), Is.EqualTo(_dbContext.Employees.Count(x => x.EmployeeId > 1)));
        }


        [Test]
        public async Task TsetCountAsyncWithPredicate()
        {
            Assert.That(await _unitOfWork.EmployeeRepository.CountAsync(x => x.EmployeeId > 1), Is.EqualTo(await _dbContext.Employees.CountAsync(x => x.EmployeeId > 1)));
        }

        #endregion


    }
}