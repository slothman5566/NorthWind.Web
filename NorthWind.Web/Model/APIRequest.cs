using Northwind.Utility;
using static Northwind.Utility.SD;

namespace Northwind.Web.Model
{
    public class APIRequest
    {
        public APIType ApiType { get; set; } = APIType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
