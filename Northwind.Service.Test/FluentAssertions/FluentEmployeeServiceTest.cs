using FluentAssertions;
using Northwind.Data.Models;
using Northwind.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Service.Test.FluentAssertions
{
    public class FluentEmployeeServiceTest : EmployeeServiceTest
    {
        [Test]
        public override void TestGetAll()
        {

            var result = _employeeService.GetAll().ToList();
            result.Count().Should().Be(3);
        }


        [Test]
        public override void TestFindBy()
        {

            var id = 1;
            var result = _employeeService.FindBy(x => x.EmployeeId == id).First();
            result.Should().NotBeNull().And.BeEquivalentTo(_employees.First());

        }
        [Test]
        public override void TestDeleteNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _employeeService.Delete(null));
            exception.ParamName.Should().NotBeNullOrEmpty().And.Be("entity");

        }

        [Test]
        public override void TestDelete()
        {
            var answer = _employees.First();
            _employeeService.Delete(_employees.First());
            var list = _employeeService.GetAll().ToList();
            list.Count().Should().Be(2);
        
        }

        [Test]
        public override void TestDeleteNotExist()
        {
            _employeeService.Delete(new Employee() { EmployeeId = 4 });
            var list = _employeeService.GetAll().ToList();
            list.Count().Should().Be(3);
        }

        [Test]
        public override void TestDeleteRange()
        {
            _employeeService.DeleteRange(new List<Employee>() {
                new Employee() { EmployeeId=1},
                new Employee() { EmployeeId=2},
                new Employee() { EmployeeId=3}

            });

            var list = _employeeService.GetAll().ToList();
            list.Count().Should().Be(0);

        }

        [Test]
        public override void TestInsertExistID()
        {
            var exception = Assert.Throws<InvalidOperationException>(() => _employeeService.Insert(new Employee() { EmployeeId = 1 }));
            exception.Message.Should().Be("DuplicateKey");

        }

        [Test]
        public override void TestInsert()
        {

            var result = _employeeService.Insert(new Employee() { EmployeeId = 4 });
           

            var list = _employeeService.GetAll().ToList();

            result.EmployeeId.Should().Be(4);
            list.Count.Should().Be(4);

        }


        [Test]
        public virtual void TestEdit()
        {
            var source = new Employee() { EmployeeId = 1, City = "BBB" };
            var result = _employeeService.Edit(source);

            result.EmployeeId.Should().Be(1);
            result.City.Should().Be("BBB");
         
        }



        [Test]
        public override async Task TestInsertAsync()
        {
            var result = await _employeeService.InsertAsync(new Employee() { EmployeeId = 4 });
            var list = _employeeService.GetAll().ToList();
            result.EmployeeId.Should().Be(4);
            list.Count.Should().Be(4);
          

        }

        [Test]
        public override async Task TestDeleteAsync()
        {
            await _employeeService.DeleteAsync(_employees.First());
            var list = _employeeService.GetAll().ToList();
            list.Count.Should().Be(2);
        }

        [Test]
        public virtual async Task TestDeleteRangeAsync()
        {
            await _employeeService.DeleteRangeAsync(new List<Employee>() {
                new Employee() { EmployeeId=1},
                new Employee() { EmployeeId=2},
                new Employee() { EmployeeId=3}

            });

            var list = _employeeService.GetAll().ToList();
            list.Count.Should().Be(0);
        }

        [Test]
        public override async Task TestEditAsync()
        {
            var source = new Employee() { EmployeeId = 1, City = "BBB" };
            var result = await _employeeService.EditAsync(source);
            result.EmployeeId.Should().Be(1);
            result.City.Should().Be("BBB");
           

        }
    }
}
