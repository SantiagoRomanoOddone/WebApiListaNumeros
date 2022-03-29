using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApiListaNumeros.Controllers
{   
    [Route("v1/minipompom")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("basic/list")]
        public async Task<IActionResult> GetBasic()
        {
            Random rnd = new Random();
            var list = rnd.Next();
            return Ok(list);
        }

        [HttpGet("jwt/list")]
        public async Task<IActionResult> GetBrearer()
        {
            Random rnd = new Random();
            var list = rnd.Next();
            return Ok(list);
        }
    }
}
