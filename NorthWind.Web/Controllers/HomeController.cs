using Microsoft.AspNetCore.Mvc;
using Northwind.Repo;
using Northwind.Service;
using Northwind.Data.Models;

namespace Northwind.Web.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeService _EmployeeService;

        public HomeController(IEmployeeService employeeService)
        {
            _EmployeeService = employeeService;
        }

        public IActionResult Index()
        {
            var list = _EmployeeService.GetAll().ToList();
            return View(list);
        }

        [HttpGet("{id}", Name = "Get")]
        public Employee Get(int id)
        {
            return _EmployeeService.FindBy(e => e.EmployeeId == id).FirstOrDefault();
        }
    }
}
