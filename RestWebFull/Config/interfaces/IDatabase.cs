using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebFull.Domain.Config
{
    public interface IDatabaseConfig
    {
        string ConnectionString { get; set; }
    }
}
