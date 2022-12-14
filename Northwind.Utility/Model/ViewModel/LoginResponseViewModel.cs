using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Utility.Model.ViewModel
{
    public class LoginResponseViewModel
    {
        public UserViewModel User { get; set; }
        public string Token { get; set; }
        public string Role { get;set; }
    }
}
