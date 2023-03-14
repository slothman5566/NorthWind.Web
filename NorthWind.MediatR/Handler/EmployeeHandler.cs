using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Data.Models;
using Northwind.Repo;
using  NorthWind.MediatR.Request;

namespace Northwind.MediatR.Handler
{



    public class GetEmployeeHandler : IRequestHandler<GetEmployeeRequest, Employee>
    {

        private readonly IUnitOfWork _unitOfWork;
        public GetEmployeeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Employee> Handle(GetEmployeeRequest request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetAll().FirstOrDefaultAsync(x => x.EmployeeId == request.EmployeeId, cancellationToken);


            return employee;
        }
    }

    public class GetEmployeesHandler : IRequestHandler<GetEmployeesRequest, IEnumerable<Employee>>
    {

        private readonly IUnitOfWork _unitOfWork;
        public GetEmployeesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Employee>> Handle(GetEmployeesRequest request, CancellationToken cancellationToken)
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAll().ToListAsync();


            return employees;
        }
    }
}
