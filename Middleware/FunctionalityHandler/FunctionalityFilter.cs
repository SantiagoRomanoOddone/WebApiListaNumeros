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
                // TODO: chequear si está nula la información antes. VALIDAR SI HORA ACTUAL ES MAYOR QUE HORA CHACHE. GUARDAR CON KEY REPRESENTATIVA (metodo y path)
                //cache
                if (!_memoryCache.TryGetValue(CacheKeys.CHACHEKEYNAME, out var extractedApiKey))
                {
                    var cacheEntryOption = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                        //Generar una caché sin vencimiento que se actualice al mismo instante que cuando vence la caché de 10 min
                        .SetAbsoluteExpiration(TimeSpan.FromHours(30));
                   
                    _memoryCache.Set(CacheKeys.CHACHEKEYNAME, data, cacheEntryOption);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

    }
}
