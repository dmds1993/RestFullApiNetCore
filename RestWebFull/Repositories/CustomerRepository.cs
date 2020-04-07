using Microsoft.EntityFrameworkCore;
using RestWebFull.Domain;
using RestWebFull.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            var result = await packDbContext.Customers.ToListAsync();
            return result.ToList();
        }

        public async Task Add(Customer customer)
        {
            await packDbContext.AddAsync<Customer>(customer);
            await Save();
        }

        public async Task Delete(Guid id)
        {
            Customer customer = await GetById(id);
            packDbContext.Remove<Customer>(customer);
            await Save();
        }

        public async Task<Customer> GetById(Guid id)
        {
            return await packDbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
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
