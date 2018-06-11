using System;

namespace WebApplication_MultiTenant.Models
{
    public class Customer
    {
        public string Name { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string Website { get; set; }
    }
}