using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Middlewares.ExceptionHandler;
using Middlewares.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Middlewares.Auxiliaries;
using System.Net;

namespace Middlewares.SecurityDisponibilityHandler
{
    public class SecurityFilter : ISecurityFilter
    {       
        public async Task SecurityCheck(HttpContext context)
        {
            try
            {
                var user = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").First();

                if (context.Request.Path == "/v1/minipompom/basic/list" && user == "Basic")
                {
                    await BasicSecurityResponseAsync();
                }
                else if (context.Request.Path == "/v1/minipompom/jwt/list" && user == "Bearer")
                {
                    await BearerSecurityResponseAsync();
                }
                else
                {
                    throw new UnauthorizedAccessException("Unauthorized User for this endpoint");
                }          
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }

            async Task BasicSecurityResponseAsync()
            {
                try
                {
                    string auth = context.Request.Headers["Authorization"].ToString().Split(new char[] { ' ' })[1];
                    Encoding encoding = Encoding.GetEncoding("UTF-8");
                    var usernameAndPassword = encoding.GetString(Convert.FromBase64String(auth));
                    string username = usernameAndPassword.Split(new char[] { ':' })[0];
                    string password = usernameAndPassword.Split(new char[] { ':' })[1];
                    if (username != "Admin" || password != "Admin123")
                    {
                        throw new UnauthorizedAccessException("Email or password is incorrect");
                    }
                }
                catch (Exception ex)
                {
                    throw new UnauthorizedAccessException(ex.Message);
                }              
                await Task.CompletedTask;
            }

            async Task BearerSecurityResponseAsync()
            {               
                try
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(Constant.Bearer.KEY);

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = Constant.ISSUER,
                        ValidAudience = Constant.AUDIENCE,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var accountId = jwtToken.Claims.First(x => x.Type == "id").Value;
                }
                catch (Exception ex)
                {
                    throw new UnauthorizedAccessException(ex.Message);
                }
                await Task.CompletedTask;
            }
        }       
    }
}
