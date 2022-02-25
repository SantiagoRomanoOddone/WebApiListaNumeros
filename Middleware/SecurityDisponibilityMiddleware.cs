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
        private readonly ISecurityDisponibilityFilter _securityDisponibilityFilter;
        private readonly IMemoryCache _memoryCache;
        public const string CHACHEKEYNAME = "CacheKey";

        public SecurityDisponibilityMiddleware(RequestDelegate next, ISecurityDisponibilityFilter securityDisponibilityFilter, IMemoryCache memoryCache)
        {
            _next = next;
            _securityDisponibilityFilter = securityDisponibilityFilter;
            _memoryCache = memoryCache;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _memoryCache.Get<Root>(CHACHEKEYNAME);               
                await _securityDisponibilityFilter.DisponibilityCheck(context);
                await _securityDisponibilityFilter.SecurityCheck(context);

                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }              
    }
}
