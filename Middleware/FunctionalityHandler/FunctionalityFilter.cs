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

                var uri = new Uri("https://be2d9e2a-4c0f-41bb-ab02-b8731ec4654c.mock.pstmn.io?");
                var request = new HttpRequestMessage
                (
                HttpMethod.Get,

                $"{uri}channel={context.Request.Headers["Channel"]}&method={context.Request.Method}&endpoint={context.Request.Path}"

                );
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Something Went Wrong! Error Ocurred");
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                context.Items.Add("functionality-response", responseBody);

                Root data = JsonConvert.DeserializeObject<Root>(responseBody);

                OnCache(data);

            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }
        public void OnCache(Root data)
        {
            var CurrentDateTime = DateTime.Now;
            if (!_memoryCache.TryGetValue(CacheKeys.CHACHEKEYTIME, out DateTime cacheValue))
            {
                cacheValue = CurrentDateTime;
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));
                _memoryCache.Set(CacheKeys.CHACHEKEYNAME, data, cacheEntryOptions);
                _memoryCache.Set(CacheKeys.CHACHEKEYTIME, cacheValue, cacheEntryOptions);
              
            }
            var CacheCurrentDateTime = cacheValue;
            if (CurrentDateTime >= CacheCurrentDateTime)
            {
                var cacheEntryOptions2 = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddYears(2));
                _memoryCache.Set(CacheKeys.CHACHEKEYNAME2, data, cacheEntryOptions2);
            }

        }
    }
}
