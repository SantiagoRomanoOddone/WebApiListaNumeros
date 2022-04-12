using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Middlewares.RequestResponseLoggingHandler;
using Telemetry;

namespace Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly IRequestLogging _requestLogging;
        private readonly IResponseLogging _responseLogging;

        public RequestResponseLoggingMiddleware(RequestDelegate next,
                                                ILoggerFactory loggerFactory, IRequestLogging requestLogging, IResponseLogging responseLogging)
        {
            _next = next;
            _requestLogging = requestLogging;
            _responseLogging = responseLogging;
            _logger = loggerFactory
                      .CreateLogger<RequestResponseLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }
        public async Task Invoke(HttpContext context)
        {
            BaggageInfo.SetBaggages(context);

            using (_logger.BeginScope("Scope: HTTP Request"))
            {
                await _requestLogging.LogRequest();
            }

            using (_logger.BeginScope("Scope: HTTP Response")) 
            {
                var originalBodyStream = context.Response.Body;
                await using var responseBody = _recyclableMemoryStreamManager.GetStream();
                context.Response.Body = responseBody;

                await _next(context);

                await _responseLogging.LogResponse();
                await responseBody.CopyToAsync(originalBodyStream);
            }      

        }
    }
}