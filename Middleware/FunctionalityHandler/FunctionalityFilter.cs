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
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMemoryCache _memoryCache;
       
        public FunctionalityFilter(IHttpClientFactory clientFactory, IMemoryCache memoryCache)
        {
            _clientFactory = clientFactory;
            _memoryCache = memoryCache;    
        }        
        public async Task FunctionalityCheck(HttpContext context)
        {
            var uri = new Uri("https://b4e7fdc5-a74d-4355-b165-8ee778b719b7.mock.pstmn.io?");
            var client = _clientFactory.CreateClient();
            var CurrentDateTime = DateTime.Now;
            try
            {
                if (context.Request.Path == "/v1/minipompom/basic/list")
                {
                    if (!_memoryCache.TryGetValue(CacheKeys.CHACHEKEYTIMEBASIC, out DateTime cacheValue))
                    {
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
                        SetCacheBasic(responseBody, CurrentDateTime);
                    }
                    else
                    {
                        if (CurrentDateTime < cacheValue + Convert.ToDateTime("00:10").TimeOfDay)
                        {
                            GetCacheBasic();
                        }
                        else
                        {
                            var request = new HttpRequestMessage
                            (
                            HttpMethod.Get,
                            $"{uri}channel={context.Request.Headers["Channel"]}&method={context.Request.Method}&endpoint={context.Request.Path}");
                            var response = await client.SendAsync(request);
                            if (!response.IsSuccessStatusCode)
                            {
                                GetCacheBasic();
                            }
                            else
                            {
                                var responseBody = await response.Content.ReadAsStringAsync();
                                SetCacheBasic(responseBody, CurrentDateTime);
                            }
                        }
                    }
                }
                else if (context.Request.Path == "/v1/minipompom/jwt/list")
                {
                    if (!_memoryCache.TryGetValue(CacheKeys.CHACHEKEYTIMEBEARER, out DateTime cacheValue))
                    {
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
                        SetCacheBearer(responseBody, CurrentDateTime);
                    }
                    else
                    {
                        if (CurrentDateTime < cacheValue + Convert.ToDateTime("00:10").TimeOfDay)
                        {
                            GetCacheBearer();
                        }
                        else
                        {
                            var request = new HttpRequestMessage
                            (
                            HttpMethod.Get,
                            $"{uri}channel={context.Request.Headers["Channel"]}&method={context.Request.Method}&endpoint={context.Request.Path}");
                            var response = await client.SendAsync(request);
                            if (!response.IsSuccessStatusCode)
                            {
                                GetCacheBearer();
                            }
                            else
                            {
                                var responseBody = await response.Content.ReadAsStringAsync();
                                SetCacheBearer(responseBody, CurrentDateTime);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            void SetCacheBasic(string responseBody, DateTime CurrentDateTime)
            {
                Root data = JsonConvert.DeserializeObject<Root>(responseBody);
                DateTime cacheValue = CurrentDateTime;
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddYears(2));
                _memoryCache.Set(CacheKeys.CHACHEKEYNAMEBASIC, data, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.CHACHEKEYTIMEBASIC, cacheValue, cacheEntryOptions);
                context.Items.Add("functionality-response", responseBody);
            }
            void GetCacheBasic()
            {
                var cachedata = JsonConvert.SerializeObject(_memoryCache.Get<Root>(CacheKeys.CHACHEKEYNAMEBASIC));
                context.Items.Add("functionality-response", cachedata);
                Root data = JsonConvert.DeserializeObject<Root>(cachedata);
            }
            void SetCacheBearer(string responseBody, DateTime CurrentDateTime)
            {
                Root data = JsonConvert.DeserializeObject<Root>(responseBody);
                DateTime cacheValue = CurrentDateTime;
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddYears(2));
                _memoryCache.Set(CacheKeys.CHACHEKEYNAMEBEARER, data, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.CHACHEKEYTIMEBEARER, cacheValue, cacheEntryOptions);
                context.Items.Add("functionality-response", responseBody);
            }
            void GetCacheBearer()
            {
                var cachedata = JsonConvert.SerializeObject(_memoryCache.Get<Root>(CacheKeys.CHACHEKEYNAMEBEARER));
                context.Items.Add("functionality-response", cachedata);
                Root data = JsonConvert.DeserializeObject<Root>(cachedata);
            }

        }      
              
    }
}
