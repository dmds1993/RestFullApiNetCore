using RestWebFull.Models;
using RestWebFull.Repositories;
using System;
using System.Threading.Tasks;

namespace RestWebFull.Services
{
    public class CreatorCustomer : ICreatorCustomer
    {
        private readonly ICustomerRepository customerRepository;
        public CreatorCustomer(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        public async Task Create(CustomerModel customer)
        {
            await customerRepository.Add(new Domain.Customer
            {
                Id = Guid.NewGuid(),
                Age = customer.Age,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname
            });
        }
    }
}
