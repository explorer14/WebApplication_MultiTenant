using ApplicationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebApplication_MultiTenant.Models;

namespace WebApplication_MultiTenant.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerService customerSvc;

        public HomeController(ICustomerService customerService)
        {
            this.customerSvc = customerService;
        }

        [Authorize]
        public IActionResult Index()
        {
            List<Customer> customersForTenant = new List<Customer>();
            var customers = this.customerSvc.GetAllCustomers();

            if (customers != null)
            {
                foreach (var customer in customers)
                {
                    customersForTenant.Add(new Customer() { Name = customer.Name, RegistrationDate = customer.RegistrationDate, Website = customer.Website });
                }
            }

            return View(customersForTenant.AsEnumerable());
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}