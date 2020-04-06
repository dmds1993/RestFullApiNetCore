using RestWebFull.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebFull.Services
{
    public interface ICustomerReader
    {
        Task<IEnumerable<CustomerDto>> ListAll();
        Task<CustomerDto> GetById(Guid id);
    }
}
