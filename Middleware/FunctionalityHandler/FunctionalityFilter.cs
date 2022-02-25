using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Middlewares.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.FunctionalityHandler
{
    public class FunctionalityFilter : IFunctionalityFilter
    {
        public const string CHACHEKEYNAME = "CacheKey";
        public async Task FunctionalityCheck(HttpContext context, IHttpClientFactory clientFactory, IMemoryCache memoryCache, IConfiguration configuration)
        {
            try
            {
                var request = new HttpRequestMessage
                (
                HttpMethod.Get,
                $"{configuration["UrlMock:url"]}?channel={context.Request.Headers["Channel"]}&method={context.Request.Method}&endpoint={context.Request.Path}"
                );
                var client = clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Something Went Wrong! Error Ocurred");
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                context.Items.Add("functionality-response", responseBody);
                Root data = JsonConvert.DeserializeObject<Root>(responseBody);

                //cache
                if (!context.Request.Headers.TryGetValue(CHACHEKEYNAME, out var extractedApiKey))
                {
                    var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                    memoryCache.Set(CHACHEKEYNAME, data, options);
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }
    }
}
