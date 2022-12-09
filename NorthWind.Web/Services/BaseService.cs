using Azure.Core;
using Newtonsoft.Json;
using Northwind.Web.Model;
using Northwind.Web.Services.IServices;
using System.Text;

namespace Northwind.Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse ResponseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory clientFactory)
        {
            ResponseModel=new APIResponse();
            httpClient = clientFactory;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
                var message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if(apiRequest.Data!= null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }

                switch (apiRequest.ApiType)
                {
                    case Northwind.Utility.SD.APIType.GET:
                        message.Method=HttpMethod.Get;
                        break;
                    case Northwind.Utility.SD.APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case Northwind.Utility.SD.APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case Northwind.Utility.SD.APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                  
                }
                var response=await client.SendAsync(message);
                var context=await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<T>(context);
                return apiResponse;
            }
            catch (Exception e)
            {

                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                return JsonConvert.DeserializeObject<T>(res);
            }
        }
    }
}
