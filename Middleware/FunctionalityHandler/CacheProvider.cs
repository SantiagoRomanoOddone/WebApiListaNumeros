using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Middlewares.Models;
using Newtonsoft.Json;

namespace Middlewares.FunctionalityHandler
{
    public class CacheProvider : ICacheProvider
    {
        public string CHACHEKEYTIME;
        public string CHACHEKEYNAME;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _clientFactory;
        public CacheProvider(IHttpClientFactory clientFactory, IMemoryCache memoryCache)
        {
            _clientFactory = clientFactory;
            _memoryCache = memoryCache;
        }

        public async Task FunctionalityCheckAsync(HttpContext context)
        {
            SemaphoreSlim GetUsersSemaphore = new SemaphoreSlim(1, 1);
            var CurrentDateTime = DateTime.Now;

            await GetCacheKey();          
            bool isAvaiable = _memoryCache.TryGetValue(CHACHEKEYTIME, out DateTime cacheValue);
            if (isAvaiable)
            {
                if (CurrentDateTime < cacheValue + Convert.ToDateTime("00:10").TimeOfDay)
                {
                    await GetCache();
                }
                else
                {
                    await FunctionalityResponseAsync(GetUsersSemaphore, cacheValue, CurrentDateTime);
                }
            }
            else
            {
                await FunctionalityResponseAsync(GetUsersSemaphore, cacheValue, CurrentDateTime);
            }         

            async Task GetCacheKey()
            {
                if (context.Request.Path == "/v1/minipompom/basic/list")
                {
                    CHACHEKEYNAME = "CacheKeyBasic";
                    CHACHEKEYTIME = "CacheTimeBasic";
                }
                else if (context.Request.Path == "/v1/minipompom/jwt/list")
                {
                    CHACHEKEYNAME = "CacheKeyBearer";
                    CHACHEKEYTIME = "CacheTimeBearer";
                }
            }

            async Task FunctionalityResponseAsync(SemaphoreSlim semaphore, DateTime cacheValue, DateTime CurrentDateTime)
            {
                await semaphore.WaitAsync();
                //Recheck
                bool isAvaiable = _memoryCache.TryGetValue(CHACHEKEYTIME, out cacheValue);
                if (isAvaiable && CurrentDateTime < cacheValue + Convert.ToDateTime("00:10").TimeOfDay)
                {
                    await GetCache();
                }
                else
                {
                    var uri = new Uri("http://localhost:8080/");
                    var client = _clientFactory.CreateClient();
                    var request = new HttpRequestMessage
                    (
                    HttpMethod.Get,
                    $"{uri}channel={context.Request.Headers["Channel"]}&method={context.Request.Method}&endpoint={context.Request.Path}");
                    var response = await client.SendAsync(request);
                    if (!response.IsSuccessStatusCode && !_memoryCache.TryGetValue(CHACHEKEYTIME, out cacheValue))
                    {
                        throw new Exception("Something Went Wrong! Error Ocurred");
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        await GetCache();
                    }
                    else
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        context.Items.Add("functionality-response", responseBody);
                        await SetCache(CurrentDateTime);
                    }                   
                }
                semaphore.Release();
            }

            async Task SetCache(DateTime CurrentDateTime)
            {
                Root data = JsonConvert.DeserializeObject<Root>(context.Items["functionality-response"].ToString());
                DateTime cacheValue = CurrentDateTime;
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddYears(2));
                _memoryCache.Set(CHACHEKEYNAME, data, cacheEntryOptions);
                _memoryCache.Set(CHACHEKEYTIME, cacheValue, cacheEntryOptions);
            }

            async Task GetCache()
            {
                var cachedata = JsonConvert.SerializeObject(_memoryCache.Get<Root>(CHACHEKEYNAME));
                context.Items.Add("functionality-response", cachedata);
            }          
        }
    }
}
