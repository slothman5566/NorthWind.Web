using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.Service;
using NorthWind.Data.Models;
using NorthWind.Web.Controllers;
using System.Reflection.Metadata;

namespace Northwind.Test
{
    [TestClass]
    public class HomeUnitTest
    {
        [TestMethod]
        public void TestGetAllMethod()
        {
            var service = new Mock<IEmployeeService>();
            service.Setup(t => t.GetAll()).Returns(GetEmployees);
              
            var controller = new HomeController(service.Object);

            var result = controller.Index();

            Assert.IsTrue(result is ViewResult);
            var model = (result as ViewResult).Model;
            Assert.IsTrue(model is IEnumerable<Employee>);

            Assert.AreEqual(1, (model as IEnumerable<Employee>).Count());

        }

        [TestMethod]
        public void TestGetMethod()
        {
            var service = new Mock<IEmployeeService>();
            var data = new List<Employee>
            {
                new Employee { FirstName = "AAA" },
              
            }.AsQueryable();
            var id = 0;
            service.Setup(t => t.FindBy(x=>x.EmployeeId==id)).Returns(data);

            var controller = new HomeController(service.Object);

            var result = controller.Get(id);

            Assert.IsTrue(result is Employee);

        }

        private List<Employee> GetEmployees()
        {
            return new List<Employee>() ;
        }

    }
}