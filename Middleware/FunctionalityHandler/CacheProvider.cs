using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Middlewares.Auxiliaries;
using Middlewares.Models;
using Newtonsoft.Json;

namespace Middlewares.FunctionalityHandler
{
    public class CacheProvider : ICacheProvider
    {
        private string cacheKeyName;
        private string cacheKeyTime;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _clientFactory;
        private static readonly SemaphoreSlim GetResponseSemaphore = new SemaphoreSlim(1, 1);
        public CacheProvider(IHttpClientFactory clientFactory, IMemoryCache memoryCache)
        {
            _clientFactory = clientFactory;
            _memoryCache = memoryCache;
        }

        public async Task FunctionalityCheckAsync(HttpContext context)
        {
            await GetResponseSemaphore.WaitAsync();

            var CurrentDateTime = DateTime.Now;           
            await GetCacheKey(context);        
            
            bool isAvailable = _memoryCache.TryGetValue(cacheKeyTime, out DateTime cacheValue);
            if (isAvailable && CurrentDateTime < cacheValue + Convert.ToDateTime(Constant.CACHE_REFRESH_TIME).TimeOfDay)
            {
                await GetCacheResponseAsync(context);
            }
            else
            {
                await GetFunctionalityResponseAsync(context, isAvailable);
                await SetCacheResponseAsync(CurrentDateTime, context);
            }
            GetResponseSemaphore.Release();                                    
        }

        private async Task GetCacheKey(HttpContext context)
        {
            if (context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").First() == "Basic")
            {
                cacheKeyName = Constant.Basic.CHACHEKEYNAME;
                cacheKeyTime = Constant.Basic.CHACHEKEYTIME;
            }
            else if (context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").First() == "Bearer")
            {
                cacheKeyName = Constant.Bearer.CHACHEKEYNAME;
                cacheKeyTime = Constant.Bearer.CHACHEKEYTIME;
            }
        }
        private async Task GetFunctionalityResponseAsync(HttpContext context, bool isAvaiable)
        {
            var treeResponse = await GetFunctionalityTreeAsync(context);

            if (!treeResponse.IsSuccessStatusCode && isAvaiable == false)
            {
                throw new Exception("Something Went Wrong! Error Ocurred");
            }
            if (!treeResponse.IsSuccessStatusCode)
            {
                await GetCacheResponseAsync(context);
            }
            else
            {
                var responseBody = await treeResponse.Content.ReadAsStringAsync();
                context.Items.Add("functionality-response", responseBody);               
            }
        }
        private async Task<HttpResponseMessage> GetFunctionalityTreeAsync(HttpContext context)
        {
            var uri = Environment.GetEnvironmentVariable("urlMock");
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage
            (
            HttpMethod.Get,
            $"{uri}channel={context.Request.Headers["Channel"]}&method={context.Request.Method}&endpoint={context.Request.Path}");
            var response = await client.SendAsync(request);
            return response;
        }
        private async Task SetCacheResponseAsync(DateTime CurrentDateTime, HttpContext context)
        {
            Root data = JsonConvert.DeserializeObject<Root>(context.Items["functionality-response"].ToString());
            DateTime cacheValue = CurrentDateTime;
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddYears(2));
            _memoryCache.Set(cacheKeyName, data, cacheEntryOptions);
            _memoryCache.Set(cacheKeyTime, cacheValue, cacheEntryOptions);
        }       
        private async Task GetCacheResponseAsync(HttpContext context)
        {
            var cachedata = JsonConvert.SerializeObject(_memoryCache.Get<Root>(cacheKeyName));
            context.Items.Add("functionality-response", cachedata);
        }

    }
}
