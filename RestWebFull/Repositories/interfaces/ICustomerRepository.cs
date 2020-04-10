using RestWebFull.Domain;
using RestWebFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebFull.Repositories
{
    public interface ICustomerRepository
    {
        Task Delete(Guid id);
        Task Update(Customer customer);
        Task<IEnumerable<Customer>> GetAll(CustomerQueryParameters customerQueryParameters);
        Task Save();
        Task Add(Customer customer);
        Task<Customer> GetById(Guid id);
    }
}
