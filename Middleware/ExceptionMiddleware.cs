using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Middlewares.ExceptionHandler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares
{
    
    public class ExceptionMiddleware : ExceptionFilter
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;          
        }
        public async Task InvokeAsync(HttpContext context)
        {          
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                await SetStatusCode(context, error);               
            }
        }         
    }
}
