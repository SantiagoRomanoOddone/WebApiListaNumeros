﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Middlewares.Helpers;
using Middlewares.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly IConfiguration _configuration;

        public SecurityDisponibilityMiddleware(RequestDelegate next, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _next = next;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            #region Disponibility
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
                throw new UnauthorizedAccessException("Unauthorized! Only Weekdays from 8 am to 10 pm");
                //context.Response.StatusCode = 401;
                //await context.Response.WriteAsync
                //      ("Unauthorized! Only Weekdays from 8 am to 10 pm");
                //return;
            }
            #endregion

            #region Security
            var clientTipe = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").First();
            string dataAccess = context.Request.Headers["Authorization"];
            if (clientTipe == "Basic")
            {
                string auth = dataAccess.Split(new char[] { ' ' })[1];
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
                    throw new AppException("Email or password is incorrect");
                    //context.Response.StatusCode = 401;
                    //return;
                }
            }
            else if (clientTipe == "Bearer")
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
                var apiKey = appSettings.GetValue<string>(APIKEYNAME);

                if (token != null)
                {
                    attachAccountToContext(context, token);
                    await _next(context);
                }
                else
                {
                    return;
                }
                void attachAccountToContext(HttpContext context, string token)
                {
                    try
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                        var issuer = _configuration["Jwt:Issuer"];
                        var audience = _configuration["Jwt:Audience"];

                        tokenHandler.ValidateToken(token, new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidIssuer = issuer,
                            ValidAudience = audience,
                            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                            ClockSkew = TimeSpan.Zero
                        }, out SecurityToken validatedToken);

                        var jwtToken = (JwtSecurityToken)validatedToken;
                        var accountId = jwtToken.Claims.First(x => x.Type == "id").Value;

                    }
                    catch
                    {
                        throw new AppException("Wrong Token Validation");                      
                    }
                }
            }
            #endregion           

        }
    }
}
