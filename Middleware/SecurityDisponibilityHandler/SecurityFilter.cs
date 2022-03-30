using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Middlewares.Auxiliaries;
using Middlewares.Models;

namespace Middlewares.SecurityDisponibilityHandler
{
    public class SecurityFilter : ISecurityFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SecurityFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task SecurityCheckAsync(Root response)
        {
            if (response.data.config.security.scopelevel == "basic")
            {
                await BasicSecurityCheckAsync();
            }
            else if (response.data.config.security.scopelevel == "jwt" )
            {
                await BearerSecurityCheckAsync();
            }
        } 
        private async Task BasicSecurityCheckAsync()
        {
            string auth = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(new char[] { ' ' })[1];
            Encoding encoding = Encoding.GetEncoding("UTF-8");
            var usernameAndPassword = encoding.GetString(Convert.FromBase64String(auth));
            string username = usernameAndPassword.Split(new char[] { ':' })[0];
            string password = usernameAndPassword.Split(new char[] { ':' })[1];
            if (username != Constant.Basic.USER || password != Constant.Basic.PASSWORD)
            {
                throw new UnauthorizedAccessException("Email or password is incorrect");
            }
        }
        private async Task BearerSecurityCheckAsync()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
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
    }
}
