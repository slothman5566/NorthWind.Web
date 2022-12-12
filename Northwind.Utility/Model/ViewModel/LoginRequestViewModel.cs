using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Utility.Model.ViewModel
{
    public class LoginRequestViewModel
    {
        [StringLength(20)]
        public string UserName { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
