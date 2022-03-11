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
        public async Task SetStatusCode(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (ex)
            {
                case AppException:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedAccessException:
                    // Unauthorized error
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case KeyNotFoundException:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = ex.Message }));
        }
    }
}
