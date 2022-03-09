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

namespace Middlewares.SecurityDisponibilityHandler
{
    public class SecurityFilter : ISecurityFilter
    {       
        public async Task SecurityCheck(HttpContext context)
        {
            //var clientTipe = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").First();
            //string dataAccess = context.Request.Headers["Authorization"];
            try
            {
                if (context.Request.Path == "/v1/minipompom/basic/list")
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
                else if (context.Request.Path == "/v1/minipompom/jwt/list")
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                   Validate(token);                  
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }         
        }
        void Validate(string token)
        {
            try
            {
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
            catch
            {
                throw new UnauthorizedAccessException("Wrong Token Validation");
            }
        }
    }
}
