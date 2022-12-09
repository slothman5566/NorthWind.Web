using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Northwind.Web.Model.ViewModel
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
    }
}
