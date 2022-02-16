using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Middlewares.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Middlewares
{
    public class FunctionalityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMemoryCache _memoryCache;
        public const string CHACHEKEYNAME = "CacheKey";

        public FunctionalityMiddleware(RequestDelegate next, IHttpClientFactory clientFactory, IMemoryCache memoryCache)
        {
            _next = next;
            _clientFactory = clientFactory;
            _memoryCache = memoryCache;
        }
        public async Task InvokeAsync(HttpContext context)
        {

            //if (!context.Request.Headers.TryGetValue(CHACHEKEYNAME, out var extractedApiKey))
            //{
            //    var path = context.Request.Path;
            //    if (path == "/v1/minipompom/basic/list")
            //    {
            //        //prueba 
            //        var request = new HttpRequestMessage(
            //        HttpMethod.Get,
            //        string.Format(
            //            "https://be2d9e2a-4c0f-41bb-ab02-b8731ec4654c.mock.pstmn.io?channel=sucursal&method=GET&endpoint=/v1/minipompom/basic/list"
            //            )
            //        );
            //        var client1 = _clientFactory.CreateClient();
            //        var response1 = await client1.SendAsync(request);
            //        if (response1.IsSuccessStatusCode)
            //        {
            //            var responseBody = await response1.Content.ReadAsStringAsync();
            //            context.Items.Add("functionality-response", responseBody);
            //            Root data = JsonConvert.DeserializeObject<Root>(responseBody);
            //            // cache                

            //            var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
            //            _memoryCache.Set(CHACHEKEYNAME, data, options);

            //            // cache
            //        }

            //        await _next(context);
            //    }

            //}
            var path = context.Request.Path;
            if (path == "/v1/minipompom/basic/list")
            {

                var uri1 = new Uri("https://be2d9e2a-4c0f-41bb-ab02-b8731ec4654c.mock.pstmn.io?channel=sucursal&method=GET&endpoint=/v1/minipompom/basic/list");

                var client1 = _clientFactory.CreateClient();
                var response1 = await client1.GetAsync(uri1);

                if (response1.IsSuccessStatusCode)
                {
                    var responseBody = await response1.Content.ReadAsStringAsync();

                    context.Items.Add("functionality-response", responseBody);
                    Root data = JsonConvert.DeserializeObject<Root>(responseBody);

                    // cache                
                    if (!context.Request.Headers.TryGetValue(CHACHEKEYNAME, out var extractedApiKey))
                    {
                        var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                        _memoryCache.Set(CHACHEKEYNAME, data, options);
                    }
                    // cache

                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync
                          ("Something Went Wrong! Error Ocurred");
                    return;
                }
            }
            else if (path == "/v1/minipompom/jwt/list")
            {
                var uri2 = new Uri("https://be2d9e2a-4c0f-41bb-ab02-b8731ec4654c.mock.pstmn.io?channel=sucursal&method=GET&endpoint=/v1/minipompom/jwt/list");
                var client2 = _clientFactory.CreateClient();
                var response2 = await client2.GetAsync(uri2);
                if (response2.IsSuccessStatusCode)
                {
                    var responseBody = await response2.Content.ReadAsStringAsync();

                    context.Items.Add("functionality-response", responseBody);
                    Root data = JsonConvert.DeserializeObject<Root>(responseBody);

                    // cache                
                    if (!context.Request.Headers.TryGetValue(CHACHEKEYNAME, out var extractedApiKey))
                    {
                        var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                        _memoryCache.Set(CHACHEKEYNAME, data, options);
                    }
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync
                          ("Something Went Wrong! Error Ocurred");
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync
                      ("Something Went Wrong! Error Ocurred");
                return;
            }

        }

    }
}
