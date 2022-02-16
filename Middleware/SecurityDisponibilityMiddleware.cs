using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Middlewares.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares
{
    public class SecurityDisponibilityMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEYNAME = "ApiKey";
        public const string CHACHEKEYNAME = "CacheKey";
        private readonly IMemoryCache _memoryCache;

        public SecurityDisponibilityMiddleware(RequestDelegate next, IMemoryCache memoryCache)
        {
            _next = next;
            _memoryCache = memoryCache;
        }
        public async Task InvokeAsync(HttpContext context)
        {          
            //_memoryCache.Get<Root>(CHACHEKEYNAME);
            var functionalityResponse = context.Items["functionality-response"];
            Root response = JsonConvert.DeserializeObject<Root>(context.Items["functionality-response"].ToString());
            
            bool available = false;
            string day = DateTime.Now.DayOfWeek.ToString().ToLower().Substring(0,3);
            TimeSpan now = DateTime.Now.TimeOfDay;

            try
            {
                Include include = response.data.availability.business_hours.includes.Find(x => x.weekday == day);
                TimeSpan fromHour = Convert.ToDateTime(include.from_hour).TimeOfDay;
                TimeSpan toHour = Convert.ToDateTime(include.to_hour).TimeOfDay;

                if (include != null && fromHour < now && toHour > now)
                {
                    available = true;
                }
            }
            catch 
            {
                available = false; 
            }
            if (available == false)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync
                      ("Unauthorized! Only Weekdays from 8 am to 10 pm");
                return;
            }

            if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                string authHeader = context.Request.Headers["Authorization"];

                if (authHeader != null)
                {
                    string auth = authHeader.Split(new char[] { ' ' })[1];
                    Encoding encoding = Encoding.GetEncoding("UTF-8");
                    var usernameAndPassword = encoding.GetString(Convert.FromBase64String(auth));
                    string username = usernameAndPassword.Split(new char[] { ':' })[0];
                    string password = usernameAndPassword.Split(new char[] { ':' })[1];
                    if (username == "Admin" && password == "Admin123")
                    {
                        await _next(context);
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    return;
                }

            }
            else
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
                var apiKey = appSettings.GetValue<string>(APIKEYNAME);

                if (token != null)
                {

                    if (!apiKey.Equals(extractedApiKey))
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync
                              ("Unauthorized client");
                        return;
                    }
                    await _next(context);
                }
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync
                      ("Wrong Token");
                return;

            }

        }
    }
}
