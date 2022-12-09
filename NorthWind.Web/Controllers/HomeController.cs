using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Northwind.Web.Model;
using Northwind.Web.Model.ViewModel;
using Northwind.Web.Services.IServices;

namespace Northwind.Web.Controllers
{
    public class HomeController : Controller
    {

        private IEmployeeService _employeeService;
        public HomeController(IEmployeeService employee)
        {
            _employeeService = employee;

        }



        public async Task<IActionResult> Index()
        {
            var list = new List<EmployeeViewModel>();
            var response = await _employeeService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(response.Result.ToString());
            }
            return View(list);
        }
    }
}
