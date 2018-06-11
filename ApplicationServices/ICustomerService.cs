using DataAcess.DomainModel;
using System.Collections.Generic;

namespace ApplicationServices
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAllCustomers();
    }
}