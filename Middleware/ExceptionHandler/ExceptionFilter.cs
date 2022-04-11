using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Middlewares.Auxiliaries;
using Newtonsoft.Json;
using OpenTelemetry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Telemetry;

namespace Middlewares.ExceptionHandler
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private static readonly ActivitySource Activity = new(Constant.OPENTELEMETRY_SOURCE);

        public ExceptionFilter(IHttpContextAccessor httpContextAccessor, ILogger<ExceptionFilter> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task SetStatusCodeAsync(Exception ex)
        {            
            using var activity = Activity.StartActivity("In Exception Filter");
            activity?.SetTag(Constant.TRACE_ID_BAGGAGE, Baggage.Current.GetBaggage(Constant.TRACE_ID_BAGGAGE));

            var response = _httpContextAccessor.HttpContext.Response;
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
            _logger.LogError(ex, $"Oooops! Algo salió mal: {ex.Message}");
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = response.StatusCode,
                ErrorMessage = ex.Message
            });
            await _httpContextAccessor.HttpContext.Response.WriteAsync(result);
        }
    }
}
