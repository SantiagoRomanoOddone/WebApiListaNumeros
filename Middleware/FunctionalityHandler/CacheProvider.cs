using System;
using System.Diagnostics;
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
using Telemetry;

namespace Middlewares.FunctionalityHandler
{
    public class CacheProvider : ICacheProvider
    {          
        private string cacheKeyName;
        private string cacheKeyTime;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly SemaphoreSlim GetResponseSemaphore = new SemaphoreSlim(1, 1);
        private static readonly ActivitySource Activity = new(Constant.OPENTELEMETRY_SOURCE);

        public CacheProvider(IHttpClientFactory clientFactory, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _memoryCache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task FunctionalityCheckAsync()
        {
            await GetResponseSemaphore.WaitAsync();

            using var activity = Activity.StartActivity("In Functionality Filter", ActivityKind.Internal);
            await BaggageInfo.SetSpecificTags(activity);

            var CurrentDateTime = DateTime.Now;
            await GetCacheKey();
            bool isAvailable = _memoryCache.TryGetValue(cacheKeyTime, out DateTime cacheValue);
            if (isAvailable && CurrentDateTime < cacheValue + Convert.ToDateTime(Constant.CACHE_REFRESH_TIME).TimeOfDay)
            {
                await GetCacheResponseAsync();
            }
            else
            {
                await GetFunctionalityResponseAsync(isAvailable);
                await SetCacheResponseAsync(CurrentDateTime);
            }
            GetResponseSemaphore.Release();
        }

        private async Task GetCacheKey()
        {
            if (_httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").First() == "Basic")
            {
                cacheKeyName = Constant.Basic.CHACHEKEYNAME;
                cacheKeyTime = Constant.Basic.CHACHEKEYTIME;
            }
            else if (_httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").First() == "Bearer")
            {
                cacheKeyName = Constant.Bearer.CHACHEKEYNAME;
                cacheKeyTime = Constant.Bearer.CHACHEKEYTIME;
            }
        }
        private async Task GetFunctionalityResponseAsync(bool isAvaiable)
        {
            var treeResponse = await GetFunctionalityTreeAsync();

            if (!treeResponse.IsSuccessStatusCode && isAvaiable == false)
            {
                throw new Exception("Something Went Wrong! Error Ocurred");
            }
            if (!treeResponse.IsSuccessStatusCode)
            {
                await GetCacheResponseAsync();
            }
            else
            {
                var responseBody = await treeResponse.Content.ReadAsStringAsync();
                _httpContextAccessor.HttpContext.Items.Add("functionality-response", responseBody);
            }
        }
        private async Task<HttpResponseMessage> GetFunctionalityTreeAsync()
        {
            using var activity = Activity.StartActivity("In Mock Service GET method");
            await BaggageInfo.SetSpecificTags(activity);

            var uri = Environment.GetEnvironmentVariable("urlMock");
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage
            (
            HttpMethod.Get,
            $"{uri}channel={_httpContextAccessor.HttpContext.Request.Headers["Channel"]}&method={_httpContextAccessor.HttpContext.Request.Method}&endpoint={_httpContextAccessor.HttpContext.Request.Path}");
            var response = await client.SendAsync(request);
            return response;
        }
        private async Task SetCacheResponseAsync(DateTime CurrentDateTime)
        {
            Root data = JsonConvert.DeserializeObject<Root>(_httpContextAccessor.HttpContext.Items["functionality-response"].ToString());
            DateTime cacheValue = CurrentDateTime;
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddYears(2));
            _memoryCache.Set(cacheKeyName, data, cacheEntryOptions);
            _memoryCache.Set(cacheKeyTime, cacheValue, cacheEntryOptions);
        }
        private async Task GetCacheResponseAsync()
        {
            var cachedata = JsonConvert.SerializeObject(_memoryCache.Get<Root>(cacheKeyName));
            _httpContextAccessor.HttpContext.Items.Add("functionality-response", cachedata);
        }

    }
}
