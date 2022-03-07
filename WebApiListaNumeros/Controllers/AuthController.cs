using Microsoft.AspNetCore.Authorization;//a
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Middlewares.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApiListaNumeros.Controllers
{   
    [Route("v1/minipompom")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("basic/list")]
        public async Task <IActionResult> GetBasic()
        {
            Random rnd = new Random();
            var list = rnd.Next();
            return Ok(list);
        }


        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("jwt/list")]
        public async Task<IActionResult> GetBrearer()
        {
            Random rnd = new Random();
            var list = rnd.Next();
            return Ok(list);
        }
    }
}
