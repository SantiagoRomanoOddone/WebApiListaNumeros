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

namespace Middlewares.SecurityDisponibilityHandler
{
    public class SecurityDisponibilityFilter : ISecurityDisponibilityFilter
    {       
        public async Task DisponibilityCheck(HttpContext context)
        {
            Root response = JsonConvert.DeserializeObject<Root>(context.Items["functionality-response"].ToString());
            string day = DateTime.Now.DayOfWeek.ToString().ToLower().Substring(0, 3);
            TimeSpan now = DateTime.Now.TimeOfDay;

            try
            {
                Include include = response.data.availability.business_hours.includes.Find(x => x.weekday == day);
                TimeSpan fromHour = Convert.ToDateTime(include.from_hour).TimeOfDay;
                TimeSpan toHour = Convert.ToDateTime(include.to_hour).TimeOfDay;

                if (include != null && fromHour > now && toHour < now)
                {
                    throw new UnauthorizedAccessException("Unauthorized! Only Weekdays from 8 am to 10 pm");      
                }               
            }
            catch (Exception ex)
            {
                throw ex;                
            }
        }

        public async Task SecurityCheck(HttpContext context)
        {
            var clientTipe = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").First();
            string dataAccess = context.Request.Headers["Authorization"];

            try
            {
                if (clientTipe == "Basic")
                {
                    string auth = dataAccess.Split(new char[] { ' ' })[1];
                    Encoding encoding = Encoding.GetEncoding("UTF-8");
                    var usernameAndPassword = encoding.GetString(Convert.FromBase64String(auth));
                    string username = usernameAndPassword.Split(new char[] { ':' })[0];
                    string password = usernameAndPassword.Split(new char[] { ':' })[1];
                    if (username != "Admin" || password != "Admin123")
                    {
                        throw new AppException("Email or password is incorrect");
                    }
                }
                else if (clientTipe == "Bearer")
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    attachAccountToContext(context, token);

                    void attachAccountToContext(HttpContext context, string token)
                    {
                        try
                        {
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var key = Encoding.ASCII.GetBytes("SecretKeywqewqeqqqqqqqqqqqweeeeeeeeeeeeeeeeeeeqweqe");
                            const string issuer = "https://localhost:44393";
                            const string audience = "https://localhost:44388";

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #region FeedBack I,portante !
            //bool available = false;
            //var clientTipe = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").First();
            //string dataAccess = context.Request.Headers["Authorization"];
            //if (clientTipe == "Basic")
            //{
            //    string auth = dataAccess.Split(new char[] { ' ' })[1];
            //    Encoding encoding = Encoding.GetEncoding("UTF-8");
            //    var usernameAndPassword = encoding.GetString(Convert.FromBase64String(auth));
            //    string username = usernameAndPassword.Split(new char[] { ':' })[0];
            //    string password = usernameAndPassword.Split(new char[] { ':' })[1];
            //    if (username == "Admin" && password == "Admin123")
            //    {
            //        available = true;
            //        //return context;
            //    }
            //    else
            //    {
            //        available = false;
            //        throw new AppException("Email or password is incorrect");
            //    }
            //}
            //else if (clientTipe == "Bearer")
            //{
            //    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            //    //var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            //    //var apiKey = appSettings.GetValue<string>(APIKEYNAME);

            //    if (token != null)
            //    {
            //        attachAccountToContext(context, token);
            //        available = true;
            //        //return context;
            //    }
            //    else
            //    {
            //        throw new UnauthorizedAccessException("Unauthorized!");
            //    }
            //    void attachAccountToContext(HttpContext context, string token)
            //    {
            //        try
            //        {
            //            var tokenHandler = new JwtSecurityTokenHandler();

            //            var key = Encoding.ASCII.GetBytes("SecretKeywqewqeqqqqqqqqqqqweeeeeeeeeeeeeeeeeeeqweqe");
            //            const string issuer = "https://localhost:44393";
            //            const string audience = "https://localhost:44388";

            //            //var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            //            //var issuer = _configuration["Jwt:Issuer"];
            //            //var audience = _configuration["Jwt:Audience"];

            //            tokenHandler.ValidateToken(token, new TokenValidationParameters
            //            {
            //                ValidateIssuerSigningKey = true,
            //                IssuerSigningKey = new SymmetricSecurityKey(key),
            //                ValidateIssuer = true,
            //                ValidateAudience = true,
            //                ValidIssuer = issuer,
            //                ValidAudience = audience,
            //                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            //                ClockSkew = TimeSpan.Zero
            //            }, out SecurityToken validatedToken);

            //            var jwtToken = (JwtSecurityToken)validatedToken;
            //            var accountId = jwtToken.Claims.First(x => x.Type == "id").Value;
            //        }
            //        catch
            //        {
            //            throw new AppException("Wrong Token Validation");
            //        }
            //    }

            //}
            //return available;
            //return context;
            #endregion
        }
    }
}
