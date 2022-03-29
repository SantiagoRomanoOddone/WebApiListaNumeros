using Microsoft.AspNetCore.Http;
using Middlewares.Models;
using Middlewares.SecurityDisponibilityHandler;
using Newtonsoft.Json;
using System;
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
            try
            {
                await Task.WhenAll(
                   Task.Run(() => _disponibilityFilter.DisponibilityCheckAsync(context)),
                   Task.Run(() => _securityFilter.SecurityCheckAsync(context)));

                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }         
        }
    }
}
