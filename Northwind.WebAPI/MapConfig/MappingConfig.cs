
using Mapster;
using Northwind.Data.Models;
using Northwind.Utility.Model.ViewModel;


namespace Northwind.WebAPI.MapConfig
{


    public class MyRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Employee, EmployeeViewModel>()
                .Map(dest => dest.ReportName, src => src.ReportsToNavigation == null ? string.Empty : src.ReportsToNavigation.FirstName);
        }
    }
}
