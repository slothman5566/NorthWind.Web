using MediatR;
using Northwind.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.MediatR.Request
{

    public class GetEmployeeRequest : IRequest<Employee>
    {

        public int EmployeeId { get; set; }

    }

    public class GetEmployeesRequest : IRequest<IEnumerable<Employee>>
    {



    }



}
