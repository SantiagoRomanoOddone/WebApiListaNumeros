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
    public class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ExceptionHandler.IExceptionFilter _exceptionFilter;

        public ExceptionMiddleware(RequestDelegate next, ExceptionHandler.IExceptionFilter exceptionFilter)
        {
            _next = next;
            _exceptionFilter = exceptionFilter;
        }
        public async Task InvokeAsync(HttpContext context)
        {          
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await _exceptionFilter.SetStatusCodeAsync(context, ex);               
            }
        }         
    }
}
