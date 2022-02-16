using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Middleware.ErrorConfig;
using Middlewares.Helpers;
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
        // Prueba 
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
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case AppException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;

                }
                 await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = error?.Message }));
                //var result = JsonSerializer.Serialize(new { message = error?.Message });
                //await response.WriteAsync(result);
            }
        }         
    }
}
