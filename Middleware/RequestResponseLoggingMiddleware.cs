using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Newtonsoft.Json;

namespace Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        public RequestResponseLoggingMiddleware(RequestDelegate next,
                                                ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory
                      .CreateLogger<RequestResponseLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }
        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            using (_logger.BeginScope("Scope: HTTP Request"))
            {
                _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +                                       
                                       $"http_schema:{context.Request.Scheme} " +
                                       $"http_host: {context.Request.Host} " +
                                       $"http_request_path: {context.Request.Path} " +
                                       $"http_request_method: {context.Request.Method} " +
                                       $"http_request_headers: {GetAllRequestHeaders(context)} " +
                                       $"http_request_body: {ReadStreamInChunks(requestStream)} " +
                                       $"http_request_query_string: {context.Request.QueryString} ");
            }
            context.Request.Body.Position = 0;

        }
        private async Task LogResponse(HttpContext context) 
        {
            // the trick to reading the response body is replacing the stream being used with a new MemoryStream and then copying the data back to the original body steam.
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            using (_logger.BeginScope("Scope: HTTP Response"))
            {            
                await _next(context);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation($"Http Response Information:{Environment.NewLine}" +
                                       $"http_response_body: {text} " +
                                       $"http_response_headers: {GetAllResponseHeaders(context)} " +
                                       $"http_response_status_code: {context.Response.StatusCode} ");

            }
            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }

        private static string GetAllRequestHeaders(HttpContext context)
        {
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            foreach (var header in context.Request.Headers )
            {
                requestHeaders.Add(header.Key, header.Value);
            }
            return JsonConvert.SerializeObject(requestHeaders);      
        }

        private static string GetAllResponseHeaders(HttpContext context)
        {
            Dictionary<string, string> responseHeaders = new Dictionary<string, string>();
            foreach (var header in context.Response.Headers)
            {
                responseHeaders.Add(header.Key, header.Value);
            }
            return JsonConvert.SerializeObject(responseHeaders);
        }
    }
}
