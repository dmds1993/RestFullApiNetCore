using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebFull.Middlewares
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        private readonly MyConfiguration myConfiguration;
        public CustomMiddleware(RequestDelegate requestDelegate, IOptions<MyConfiguration> myConfiguration)
        {
            this.requestDelegate = requestDelegate;
            this.myConfiguration = myConfiguration.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Debug.WriteLine($"-----> Request asked for {httpContext.Request.Path}");

            await requestDelegate.Invoke(httpContext);
        }
    }
}
