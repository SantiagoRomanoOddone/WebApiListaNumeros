using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Middlewares.Auxiliaries;
using Middlewares.RequestResponseModels;
using Newtonsoft.Json;
using OpenTelemetry;
using Telemetry;

namespace Middlewares.RequestResponseLoggingHandler
{
    public class RequestLogging : IRequestLogging
    {
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly ActivitySource Activity = new(Constant.OPENTELEMETRY_SOURCE);
        public RequestLogging(IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory
                      .CreateLogger<RequestLogging>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task LogRequest()
        {
            using var activity = Activity.StartActivity("In Request Logging");
            BaggageInfo.EnrichBaggage(activity);

            _httpContextAccessor.HttpContext.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await _httpContextAccessor.HttpContext.Request.Body.CopyToAsync(requestStream);

            var requestInformation = await GetRequestInformation(requestStream);
            _logger.LogInformation($"Http Request Information:{Environment.NewLine} " +
                                    $"{ requestInformation}");
            _httpContextAccessor.HttpContext.Request.Body.Position = 0;
           
        }
        private async Task<string> GetRequestInformation(Stream requestStream)
        {
            var requestHeaders = await GetAllRequestHeaders();
            var requestInformation = new RequestInformation
            {
                trace_Id = Baggage.Current.GetBaggage(Constant.TRACE_ID_BAGGAGE),
                http_schema = _httpContextAccessor.HttpContext.Request.Scheme,
                http_host = (_httpContextAccessor.HttpContext.Request.Host).ToString(),
                http_request_path = _httpContextAccessor.HttpContext.Request.Path,
                http_request_method = _httpContextAccessor.HttpContext.Request.Method,
                http_request_headers = requestHeaders,
                http_request_query_string = (_httpContextAccessor.HttpContext.Request.QueryString).ToString(),
                http_request_body = (ReadStreamInChunks(requestStream))
            };
            string requestInfo = JsonConvert.SerializeObject(requestInformation);

            return requestInfo;
        }
        private async Task<string> GetAllRequestHeaders()
        {
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
            foreach (var header in _httpContextAccessor.HttpContext.Request.Headers)
            {
                requestHeaders.Add(header.Key, header.Value);
            }
            return JsonConvert.SerializeObject(requestHeaders);
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
    }
}
