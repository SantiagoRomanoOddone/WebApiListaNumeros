using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.ExceptionHandler
{
    public class ExceptionFilter : IExceptionFilter
    {
        public async Task SetStatusCode(HttpContext context, Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case AppException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedAccessException e:
                    // Unauthorized error
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    //ArgumentNullException
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

            }
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = error?.Message }));
        }
    }
}
