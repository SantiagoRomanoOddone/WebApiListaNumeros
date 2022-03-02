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
        private readonly IFunctionalityFilter _functionalityFilter;

        public FunctionalityMiddleware(RequestDelegate next, IFunctionalityFilter functionalityFilter )
        {
            _next = next;
            _functionalityFilter = functionalityFilter;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _functionalityFilter.FunctionalityCheck(context);
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }                   
        }

    }
}
