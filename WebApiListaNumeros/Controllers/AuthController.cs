using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebApiListaNumeros.Controllers
{   
    [Route("v1/minipompom")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger _logger;

        public AuthController (ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet("basic/list")]
        public async Task<IActionResult> GetBasic()
        {     
            Random rnd = new Random();
            var list = rnd.Next();
            _logger.LogInformation($"Start: Getting Basic User Information: {list}");
            return Ok(list);
            
        }

        [HttpGet("jwt/list")]
        public async Task<IActionResult> GetBrearer()
        {
            Random rnd = new Random();
            var list = rnd.Next();
            _logger.LogInformation($"Start: Getting Bearer User Information: {list}");
            return Ok(list);
        }      
    }
}
