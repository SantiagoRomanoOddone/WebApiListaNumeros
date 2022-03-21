using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Middlewares.FunctionalityHandler;
using Middlewares.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Middlewares
{
    public class FunctionalityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICacheProvider _cacheProvider;

        public FunctionalityMiddleware(RequestDelegate next, ICacheProvider cacheProvider)
        {
            _next = next;
            _cacheProvider = cacheProvider;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _cacheProvider.FunctionalityCheck(context);
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }                   
        }

    }
}
