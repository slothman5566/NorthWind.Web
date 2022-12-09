using Northwind.Web.Model.ViewModel;

namespace Northwind.Web.Services.IServices
{
    public interface IEmployeeService
    {
        Task<T> GetAllAsync<T>();
        Task<T> FindByAsync<T>(int id);
        Task<T> InsertAsync<T>(EmployeeViewModel employee);
        Task<T> UpdateAsync<T>(int id, EmployeeViewModel employee);
        Task<T> DeleteAsync<T>(int id);
    }
}
