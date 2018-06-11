using System.Collections.Generic;

namespace DataAcess.DomainModel
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAllCustomers();
    }
}