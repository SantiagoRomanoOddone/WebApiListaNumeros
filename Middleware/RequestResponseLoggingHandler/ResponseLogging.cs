using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Middlewares.Auxiliaries;
using Middlewares.RequestResponseModels;
using Newtonsoft.Json;
using OpenTelemetry;
using Telemetry;

namespace Middlewares.RequestResponseLoggingHandler
{
    public class ResponseLogging : IResponseLogging
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly ActivitySource Activity = new(Constant.OPENTELEMETRY_SOURCE);
        public ResponseLogging(IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerFactory
                      .CreateLogger<ResponseLogging>();
        }

        public async Task LogResponse()
        {
            using var activity = Activity.StartActivity("In Response Logging");
            await BaggageInfo.SetSpecificTags(activity);

            _httpContextAccessor.HttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(_httpContextAccessor.HttpContext.Response.Body).ReadToEndAsync();
            _httpContextAccessor.HttpContext.Response.Body.Seek(0, SeekOrigin.Begin);

            var responseInformation = await GetResponseInformation(text);
            _logger.LogInformation($"Http Response Information:{Environment.NewLine} " +
                                    $"{ responseInformation}");
        }

        private async Task<string> GetResponseInformation(string text)
        {
            var responseHeaders = await GetAllResponseHeaders();
            var responseInformation = new ResponseInformation
            {
                trace_Id = Baggage.Current.GetBaggage(Constant.TRACE_ID_BAGGAGE),
                http_response_body = JsonConvert.SerializeObject(text),
                http_response_headers = responseHeaders,
                http_response_status_code = (_httpContextAccessor.HttpContext.Response.StatusCode).ToString()
            };
            string responseInfo = JsonConvert.SerializeObject(responseInformation);

            return responseInfo;
        }
        private async Task<string> GetAllResponseHeaders()
        {
            Dictionary<string, string> responseHeaders = new Dictionary<string, string>();
            foreach (var header in _httpContextAccessor.HttpContext.Response.Headers)
            {
                responseHeaders.Add(header.Key, header.Value);
            }
            return JsonConvert.SerializeObject(responseHeaders);
        }
    }
}
