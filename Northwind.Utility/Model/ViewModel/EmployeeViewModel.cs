using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Utility.Model.ViewModel
{
    public class EmployeeViewModel
    {
        [Key]
        public int EmployeeId { get; set; }

        [NotNull]
        [StringLength(10)]
        public string FirstName { get; set; }

        [NotNull]
        [StringLength(20)]
        public string LastName { get; set; }

        [StringLength(30)]
        public string? Title { get; set; }

        public Guid Gid { get; set; }

        public string Test { get; set; }
    }
}
