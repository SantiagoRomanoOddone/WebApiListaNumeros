using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
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
                await _exceptionFilter.SetStatusCodeAsync(ex);               
            }
        }         
    }
}
