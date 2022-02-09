using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;//a
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiListaNumeros.Controllers;//a
using WebApiListaNumeros.Services;

namespace Middlewares
{
    class SecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEYNAME = "ApiKey";
       
        public SecurityMiddleware(RequestDelegate next)
        {
            _next = next;
           
        }
        public async Task InvokeAsync(HttpContext context)
        {
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
