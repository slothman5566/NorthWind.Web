using Microsoft.AspNetCore.Mvc;
using Northwind.Repo;
using Northwind.Service;

namespace NorthWind.Web.Controllers
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
    }
}
