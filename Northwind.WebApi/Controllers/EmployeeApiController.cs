using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Data.Models;
using Northwind.Service;
using Northwind.WebApi.Model.ViewModel;

namespace Northwind.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private IEmployeeService _employeeService;
        private IMapper _mapper;
        public EmployeeApiController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeViewModel>> GetAll()
        {
            return Ok(_mapper.Map<List<EmployeeViewModel>>(_employeeService.GetAll().ToList()));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EmployeeViewModel> FindBy(int id)
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
            return Ok(_mapper.Map<EmployeeViewModel>(result));
        }




    }
}
