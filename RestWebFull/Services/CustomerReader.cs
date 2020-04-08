using RestWebFull.Domain;
using RestWebFull.Dtos;
using RestWebFull.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebFull.Services
{
    public class CustomerReader : ICustomerReader
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerReader(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        public async Task<Customer> GetById(Guid id)
        {
            return await customerRepository.GetById(id);
        }

        public async Task<IEnumerable<CustomerDto>> ListAll()
        {
            var result = await customerRepository.GetAll();
            return result.Select(r => new CustomerDto 
            {
                Id = r.Id,
                Age = r.Age,
                Firstname = r.Firstname,
                Lastname = r.Lastname
            });
        }
    }
}
