using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebFull.Services.interfaces
{
    public interface ISeedDataService
    {
        Task EnsureSeedData();
    }
}
