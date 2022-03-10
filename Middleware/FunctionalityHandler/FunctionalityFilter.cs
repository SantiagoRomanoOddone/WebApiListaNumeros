using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Middlewares.Auxiliaries;
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
        public string CHACHEKEYNAME;
        public string CHACHEKEYTIME;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMemoryCache _memoryCache;

        public FunctionalityFilter(IHttpClientFactory clientFactory, IMemoryCache memoryCache)
        {
            _clientFactory = clientFactory;
            _memoryCache = memoryCache;    
        }
      
        public async Task FunctionalityCheck(HttpContext context)
        {
            var CurrentDateTime = DateTime.Now;                 
            try
            {
                GetCacheKey();
                if (!_memoryCache.TryGetValue(CHACHEKEYTIME, out DateTime cacheValue))
                {
                    await FirstFunctionalityResponseAsync();
                    SetCache(CurrentDateTime);
                }
                else
                {
                    if (CurrentDateTime < cacheValue + Convert.ToDateTime("00:01").TimeOfDay)
                    {
                        GetCache();
                    }
                    else
                    {
                        await FunctionalityResponseAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            void GetCacheKey()
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
            void SetCache(DateTime CurrentDateTime)
            {
                Root data = JsonConvert.DeserializeObject<Root>(context.Items["functionality-response"].ToString());
                DateTime cacheValue = CurrentDateTime;
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddYears(2));
                _memoryCache.Set(CHACHEKEYNAME, data, cacheEntryOptions);
                _memoryCache.Set(CHACHEKEYTIME, cacheValue, cacheEntryOptions);
            }
            void GetCache()
            {
                var cachedata = JsonConvert.SerializeObject(_memoryCache.Get<Root>(CHACHEKEYNAME));
                context.Items.Add("functionality-response", cachedata);
                Root data = JsonConvert.DeserializeObject<Root>(cachedata);
            }
            async Task FirstFunctionalityResponseAsync()
            {
                var uri = new Uri("https://b4e7fdc5-a74d-4355-b165-8ee778b719b7.mock.pstmn.io?");
                var client = _clientFactory.CreateClient();
                var request = new HttpRequestMessage
                (
                HttpMethod.Get,
                $"{uri}channel={context.Request.Headers["Channel"]}&method={context.Request.Method}&endpoint={context.Request.Path}");
                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Something Went Wrong! Error Ocurred");
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                context.Items.Add("functionality-response", responseBody);
            }
            async Task FunctionalityResponseAsync()
            {
                var uri = new Uri("https://b4e7fdc5-a74d-4355-b165-8ee778b719b7.mock.pstmn.io?");
                var client = _clientFactory.CreateClient();
                var request = new HttpRequestMessage
                (
                HttpMethod.Get,
                $"{uri}channel={context.Request.Headers["Channel"]}&method={context.Request.Method}&endpoint={context.Request.Path}");
                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    GetCache();
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    context.Items.Add("functionality-response", responseBody);
                    SetCache(CurrentDateTime);
                }
            }
        }
     
    }
}
