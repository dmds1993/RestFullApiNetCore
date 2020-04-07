using RestWebFull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebFull.Services
{
    public interface ICreatorCustomer
    {
        Task Create(CustomerModel customer);
    }
}
