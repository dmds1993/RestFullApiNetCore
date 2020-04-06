using Microsoft.EntityFrameworkCore;
using RestWebFull.Domain;
using RestWebFull.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebFull.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly PackDbContext packDbContext;
        public CustomerRepository(PackDbContext packDbContext)
        {
            this.packDbContext = packDbContext;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await packDbContext.Customers.ToListAsync();
        }

        public async Task Add(Customer customer)
        {
            await packDbContext.AddAsync<Customer>(customer);
        }

        public async Task Delete(Guid id)
        {
            Customer customer = await GetById(id);
            packDbContext.Remove<Customer>(customer);
            await Save();
        }

        public async Task<Customer> GetById(Guid id)
        {
            var result = await GetAll();
            return result.First();
        }

        public async Task Save()
        {
            await packDbContext.SaveChangesAsync();
        }

        public async Task Update(Customer customer)
        {
            packDbContext.Customers.Update(customer);
            await Save();
        }
    }
}
