using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Middlewares.Auxiliaries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Middlewares.ExceptionHandler
{
    public class ExceptionFilter : IExceptionFilter
    {    
        public async Task SetStatusCodeAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = Constant.RESPONSE_CONTENT_TYPE;

            switch (ex)
            {
                case AppException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;                 
                    break;
                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case SecurityTokenExpiredException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case SecurityTokenInvalidSignatureException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case ArgumentException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = response.StatusCode,
                ErrorMessage = ex.Message
            });
            await context.Response.WriteAsync(result);
        }
    }
}
