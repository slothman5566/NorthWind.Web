using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Northwind.Web.Model;
using Northwind.Web.Model.ViewModel;
using Northwind.Web.Services.IServices;

namespace Northwind.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employee)
        {
            _employeeService = employee;

        }
        public async Task<IActionResult> Insert()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Insert(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {

                var response = await _employeeService.InsertAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Success";
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["error"] = "Error.";
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _employeeService.FindByAsync<APIResponse>(id);
            if (response != null && response.IsSuccess)
            {

                var model = JsonConvert.DeserializeObject<EmployeeViewModel>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {

                var response = await _employeeService.UpdateAsync<APIResponse>(model.EmployeeId, model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Success";
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["error"] = "Error.";
            return View(model);

        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _employeeService.FindByAsync<APIResponse>(id);
            if (response != null && response.IsSuccess)
            {

                var model = JsonConvert.DeserializeObject<EmployeeViewModel>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [HttpPost()]
        public async Task<IActionResult> Delete(EmployeeViewModel model)
        {
            var response = await _employeeService.DeleteAsync<APIResponse>(model.EmployeeId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Success";
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = "Error.";
            return View(model);
        }
    }
}
