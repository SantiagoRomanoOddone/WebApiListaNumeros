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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SecurityDisponibilityMiddleware(RequestDelegate next, IDisponibilityFilter disponibilityFilter, ISecurityFilter securityFilter, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _securityFilter = securityFilter;
            _disponibilityFilter = disponibilityFilter;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                Root response = JsonConvert.DeserializeObject<Root>(_httpContextAccessor.HttpContext.Items["functionality-response"].ToString());
                await Task.WhenAll(
                   Task.Run(() => _disponibilityFilter.DisponibilityCheckAsync(response)),
                   Task.Run(() => _securityFilter.SecurityCheckAsync(response)));

                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }         
        }
    }
}
