using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServicesSample.Web.Models.Data
{
    public class Person
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string EmailAddress { get; set; }
    }
}
