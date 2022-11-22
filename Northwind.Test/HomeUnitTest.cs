using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.Service;
using NorthWind.Data.Models;
using NorthWind.Web.Controllers;

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

        private List<Employee> GetEmployees()
        {
            return new List<Employee>() ;
        }

    }
}