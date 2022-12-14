using AutoMapper;
using Northwind.Data.Models;
using Northwind.Utility.Model.ViewModel;


namespace Northwind.WebAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();

            CreateMap<LocalUser, UserViewModel>().ReverseMap();

            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}
