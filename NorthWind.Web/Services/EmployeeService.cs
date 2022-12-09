using Northwind.Utility;
using Northwind.Web.Model;
using Northwind.Web.Model.ViewModel;
using Northwind.Web.Services.IServices;

namespace Northwind.Web.Services
{
    public class EmployeeService : BaseService, IEmployeeService
    {

        private IHttpClientFactory _httpClientFactory;
        private string _url;
        public EmployeeService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _httpClientFactory = clientFactory;
            _url = configuration.GetValue<string>("APIUrls:WebAPI");
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.DELETE,
               
                Url= _url+ "/api/EmployeeAPI/"+id
            }) ;

        }

        public Task<T> FindByAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.GET,

                Url = _url + "/api/EmployeeAPI/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {

            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.GET,

                Url = _url + "/api/EmployeeAPI" 
            });
        }

        public Task<T> InsertAsync<T>(EmployeeViewModel employee)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.POST,
                Data=employee,
                Url = _url + "/api/EmployeeAPI"
            });
        }

        public Task<T> UpdateAsync<T>(int id, EmployeeViewModel employee)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.APIType.PUT,
                Data=employee,
                Url = _url + "/api/EmployeeAPI/" + id
            });
        }
    }
}
