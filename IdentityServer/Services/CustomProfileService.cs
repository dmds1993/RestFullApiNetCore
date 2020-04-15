using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class CustomProfileService : IIdentityServerBuilder
    {
        public IServiceCollection Services => throw new NotImplementedException();
    }
}
