using DataAcess.DomainModel;
using System.Collections.Generic;

namespace ApplicationServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return this.customerRepository.GetAllCustomers();
        }
    }
}