using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiListaNumeros.Controllers
{
    [Route("/v1/minipompom/jwt")]
    [ApiController]   
    public class BearerAuthController : ControllerBase
    {
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("list")]
        public IActionResult GetResult()
        {
            return Ok(" BearerAuth API");
        }
    }
}
