
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Data.Models;
using Northwind.Service.IService;
using Northwind.Utility;
using Northwind.Utility.Model;
using Northwind.Utility.Model.ViewModel;
using Northwind.WebAPI.MapConfig;
using System.Net;

namespace Northwind.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAPIController : ControllerBase
    {
        private IEmployeeService _employeeService;
        private IMapper _mapper;

        public EmployeeAPIController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles =SD.Admin)]
        public async Task<ActionResult<APIResponse>> GetAll()
        {
            var _response = new APIResponse();
            var result = await _employeeService.GetAllLazyLoad(null, x => x.ReportsToNavigation).ToListAsync();
            _response.Result = _mapper.Map<List<EmployeeViewModel>>(result);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "FindBy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> FindBy(int id)
        {

            if (id < 0)
            {
                return BadRequest("id 不能小於0");
            }
            var result = await _employeeService.FindBy(x => x.EmployeeId == id).FirstOrDefaultAsync();
            if (result == null)
            {
                return NotFound("id 不存在");
            }
            var _response = new APIResponse();


            _response.Result = _mapper.MulitMap<EmployeeViewModel>(Guid.NewGuid(), result, 10);
            _response.StatusCode = HttpStatusCode.OK;


            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Insert([FromBody] EmployeeViewModel employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (employee == null)
            {
                return BadRequest(employee);
            }

            var result = await _employeeService.InsertAsync(_mapper.Map<Employee>(employee));
            var _response = new APIResponse();
            _response.Result = _mapper.Map<EmployeeViewModel>(result);
            _response.StatusCode = HttpStatusCode.OK;
            return CreatedAtRoute("FindBy", new { id = result.EmployeeId }, _response);
        }

        [HttpDelete("{id:int}", Name = "Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            if (id < 0)
            {
                return BadRequest("id 不能小於0");
            }
            var result = _employeeService.FindBy(x => x.EmployeeId == id).FirstOrDefault();
            if (result == null)
            {
                return NotFound("id 不存在");
            }
            await _employeeService.DeleteAsync(result);
            var _response = new APIResponse();


            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        [HttpPut("{id:int}", Name = "Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Update(int id, [FromBody] EmployeeViewModel employeeViewModel)
        {
            if (id < 0)
            {
                return BadRequest("id 不能小於0");
            }
            var result = _employeeService.FindBy(x => x.EmployeeId == id).FirstOrDefault();
            if (result == null)
            {
                return NotFound("id 不存在");
            }
            employeeViewModel.EmployeeId = id;
            var _response = new APIResponse();
            result = await _employeeService.EditAsync(_mapper.Map<Employee>(employeeViewModel));
            _response.Result = _mapper.Map<EmployeeViewModel>(result);


            _response.StatusCode = HttpStatusCode.NoContent;
            return CreatedAtRoute("FindBy", new { id = result.EmployeeId }, _response);
        }
    }
}
