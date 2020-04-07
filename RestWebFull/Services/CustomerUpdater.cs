using RestWebFull.Models;
using RestWebFull.Repositories;
using System;
using System.Threading.Tasks;

namespace RestWebFull.Services
{
    public class CustomerUpdater : ICustomerUpdater
    {
        private readonly ICustomerRepository customerRepository; 
        public CustomerUpdater(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        public async Task Remove(Guid id)
        {
            await customerRepository.Delete(id);
        }

        public async Task Update(CustomerModel customer)
        {
            await customerRepository.Update(new Domain.Customer
            {
                Id = customer.Id,
                Age = customer.Age,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname
            });
        }
    }
}
