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
            try
            {
                if (context.Request.Path == "/v1/minipompom/basic/list")
                {
                    var CurrentDateTime = DateTime.Now;
                    if (!_memoryCache.TryGetValue(CacheKeys.CHACHEKEYTIMEBASIC, out DateTime cacheValue))
                    {
                        var uri = new Uri("https://b4e7fdc5-a74d-4355-b165-8ee778b719b7.mock.pstmn.io?");
                        var request = new HttpRequestMessage
                        (
                        HttpMethod.Get,
                        $"{uri}channel={context.Request.Headers["Channel"]}&method={context.Request.Method}&endpoint={context.Request.Path}");
                        var client = _clientFactory.CreateClient();
                        var response = await client.SendAsync(request);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception("Something Went Wrong! Error Ocurred");
                        }
                        var responseBody = await response.Content.ReadAsStringAsync();

                        Root data = JsonConvert.DeserializeObject<Root>(responseBody);

                        cacheValue = CurrentDateTime;

                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(DateTime.Now.AddYears(2));
                        _memoryCache.Set(CacheKeys.CHACHEKEYNAMEBASIC, data, cacheEntryOptions);
                        _memoryCache.Set(CacheKeys.CHACHEKEYTIMEBASIC, cacheValue, cacheEntryOptions);

                        context.Items.Add("functionality-response", responseBody);
                    }
                    else
                    {
                        var tenMinutes = Convert.ToDateTime("00:10").TimeOfDay;
                        var expirationTime = cacheValue + tenMinutes;

                        if (CurrentDateTime < expirationTime)
                        {
                            var cachedata = JsonConvert.SerializeObject(_memoryCache.Get<Root>(CacheKeys.CHACHEKEYNAMEBASIC));
                            context.Items.Add("functionality-response", cachedata);
                            Root data = JsonConvert.DeserializeObject<Root>(cachedata);
                        }
                        else
                        {
                            var uri = new Uri("https://b4e7fdc5-a74d-4355-b165-8ee778b719b7.mock.pstmn.io?");
                            var request = new HttpRequestMessage
                            (
                            HttpMethod.Get,
                            $"{uri}channel={context.Request.Headers["Channel"]}&method={context.Request.Method}&endpoint={context.Request.Path}");
                            var client = _clientFactory.CreateClient();
                            var response = await client.SendAsync(request);

                            if (!response.IsSuccessStatusCode)
                            {
                                var cachedata = JsonConvert.SerializeObject(_memoryCache.Get<Root>(CacheKeys.CHACHEKEYNAMEBASIC));
                                context.Items.Add("functionality-response", cachedata);
                                Root data = JsonConvert.DeserializeObject<Root>(cachedata);
                            }
                            else
                            {
                                var responseBody = await response.Content.ReadAsStringAsync();
                                Root data = JsonConvert.DeserializeObject<Root>(responseBody);
                                cacheValue = CurrentDateTime;

                                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                    .SetAbsoluteExpiration(DateTime.Now.AddYears(2));
                                _memoryCache.Set(CacheKeys.CHACHEKEYNAMEBASIC, data, cacheEntryOptions);
                                _memoryCache.Set(CacheKeys.CHACHEKEYTIMEBASIC, cacheValue, cacheEntryOptions);

                                context.Items.Add("functionality-response", responseBody);
                            }

                        }

                    }

                }
                else if (context.Request.Path == "/v1/minipompom/jwt/list")
                {
                    var CurrentDateTime = DateTime.Now;
                    if (!_memoryCache.TryGetValue(CacheKeys.CHACHEKEYTIMEBEARER, out DateTime cacheValue))
                    {
                        var uri = new Uri("https://b4e7fdc5-a74d-4355-b165-8ee778b719b7.mock.pstmn.io?");
                        var request = new HttpRequestMessage
                        (
                        HttpMethod.Get,
                        $"{uri}channel={context.Request.Headers["Channel"]}&method={context.Request.Method}&endpoint={context.Request.Path}");
                        var client = _clientFactory.CreateClient();
                        var response = await client.SendAsync(request);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception("Something Went Wrong! Error Ocurred");
                        }
                        var responseBody = await response.Content.ReadAsStringAsync();

                        Root data = JsonConvert.DeserializeObject<Root>(responseBody);

                        cacheValue = CurrentDateTime;

                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(DateTime.Now.AddYears(2));
                        _memoryCache.Set(CacheKeys.CHACHEKEYNAMEBEARER, data, cacheEntryOptions);
                        _memoryCache.Set(CacheKeys.CHACHEKEYTIMEBEARER, cacheValue, cacheEntryOptions);

                        context.Items.Add("functionality-response", responseBody);
                    }
                    else
                    {
                        var tenMinutes = Convert.ToDateTime("00:10").TimeOfDay;
                        var expirationTime = cacheValue + tenMinutes;

                        if (CurrentDateTime < expirationTime)
                        {
                            var cachedata = JsonConvert.SerializeObject(_memoryCache.Get<Root>(CacheKeys.CHACHEKEYNAMEBEARER));
                            context.Items.Add("functionality-response", cachedata);
                            Root data = JsonConvert.DeserializeObject<Root>(cachedata);
                        }
                        else
                        {
                            var uri = new Uri("https://b4e7fdc5-a74d-4355-b165-8ee778b719b7.mock.pstmn.io?");
                            var request = new HttpRequestMessage
                            (
                            HttpMethod.Get,
                            $"{uri}channel={context.Request.Headers["Channel"]}&method={context.Request.Method}&endpoint={context.Request.Path}");
                            var client = _clientFactory.CreateClient();
                            var response = await client.SendAsync(request);

                            if (!response.IsSuccessStatusCode)
                            {
                                var cachedata = JsonConvert.SerializeObject(_memoryCache.Get<Root>(CacheKeys.CHACHEKEYNAMEBEARER));
                                context.Items.Add("functionality-response", cachedata);
                                Root data = JsonConvert.DeserializeObject<Root>(cachedata);
                            }
                            else
                            {
                                var responseBody = await response.Content.ReadAsStringAsync();
                                Root data = JsonConvert.DeserializeObject<Root>(responseBody);
                                cacheValue = CurrentDateTime;

                                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                    .SetAbsoluteExpiration(DateTime.Now.AddYears(2));
                                _memoryCache.Set(CacheKeys.CHACHEKEYNAMEBEARER, data, cacheEntryOptions);
                                _memoryCache.Set(CacheKeys.CHACHEKEYTIMEBEARER, cacheValue, cacheEntryOptions);

                                context.Items.Add("functionality-response", responseBody);
                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }      
              
    }
}
