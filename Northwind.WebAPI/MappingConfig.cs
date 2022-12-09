using AutoMapper;
using Northwind.Data.Models;
using Northwind.WebAPI.Model.ViewModel;

namespace Northwind.WebAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
          

        }
    }
}
