using AutoMapper;
using Northwind.Data.Models;
using Northwind.WebApi.Model.ViewModel;

namespace Northwind.WebApi
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<EmployeeViewModel, Employee>();

        }
    }
}
