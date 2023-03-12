using MediatR;
using Microsoft.EntityFrameworkCore;
using Northwind.Data.Models;
using Northwind.Repo;

namespace Northwind.WebAPI.MediatR.Handler
{
    public class GetEmployeeRequest : IRequest<Employee>
    {

        public int EmployeeId { get; set; }

    }


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
}
