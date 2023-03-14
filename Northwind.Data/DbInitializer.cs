using Northwind.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Data
{
    public static class DbInitializer
    {

        public static void SeedData(NorthwindContext context)
        {

            if (!context.Employees.Any())
            {
                var employees = new List<Employee>() {
                new Employee() { EmployeeId = 1 ,FirstName="A", LastName="AA"},
                new Employee() { EmployeeId = 2 ,FirstName="A", LastName="BB"},
                new Employee() { EmployeeId = 3 ,FirstName="A", LastName="CC",ReportsTo=1}};

                context.AddRange(employees);
            }

            if (!context.LocalUsers.Any())
            {
                var users = new List<LocalUser>() {
                new LocalUser() {Id=1,UserName="AAA",Password="AAA",Name="AAA",Role="Admin"},
                new LocalUser() {Id=2,UserName="BBB",Password="BBB",Name="BBB",Role="Customer" },
                new LocalUser() {Id=3,UserName="CCC",Password="CCC", Name = "CCC",Role="Customer" }};
                context.AddRange(users);

            }
            context.SaveChanges();
        }
    }
}
