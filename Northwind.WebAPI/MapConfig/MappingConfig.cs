
using Mapster;
using Northwind.Data.Models;
using Northwind.Utility.Model.ViewModel;


namespace Northwind.WebAPI.MapConfig
{


    public class EmployeeViewModelMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Guid, EmployeeViewModel>()
                .Map(dest => dest.Gid, src => src);

            config.NewConfig<int, EmployeeViewModel>()
                .Map(dest => dest.Test, src => (src + 1).ToString());
        }
    }
}
