using DataAccess;
using DataAcess.DomainModel;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DataAcess
{
    public class CustomerRepository : ICustomerRepository
    {
        private CRMContext context;

        public CustomerRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return this.context?.Customers.ToList().AsEnumerable();
        }
    }
}