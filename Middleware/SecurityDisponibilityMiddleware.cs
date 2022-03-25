using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Middlewares.ExceptionHandler;
using Middlewares.Models;
using Middlewares.SecurityDisponibilityHandler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares
{
    public class SecurityDisponibilityMiddleware 
    {
        private readonly RequestDelegate _next;      
        private readonly ISecurityFilter _securityFilter;
        private readonly IDisponibilityFilter _disponibilityFilter;       

        public SecurityDisponibilityMiddleware(RequestDelegate next, IDisponibilityFilter disponibilityFilter, ISecurityFilter securityFilter)
        {
            _next = next;
            _securityFilter = securityFilter;
            _disponibilityFilter = disponibilityFilter;      
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await Task.WhenAll(
               Task.Run(() => _disponibilityFilter.DisponibilityCheckAsync(context)),
               Task.Run(() => _securityFilter.SecurityCheckAsync(context)));

            await _next.Invoke(context);
        }
    }
}
