﻿using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthWind.MediatR.Event;
using NorthWind.MediatR.Request;

namespace Northwind.WebAPI.MediatR.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetEmployeeRequest request)
        {
            await _mediator.Publish(request);
            return Ok(await _mediator.Send(request));

        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var request = new GetEmployeesRequest();
            return Ok(await _mediator.Send(request));

        }

        [HttpGet("Call")]
        public async Task<IActionResult> Call()
        {
           await _mediator.Publish(new EmployeeNotifyEvent());
            return Ok();
        }
    }
}
