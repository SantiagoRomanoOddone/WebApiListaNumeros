using Microsoft.AspNetCore.Authorization;//a
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApiListaNumeros.Filters;
using WebApiListaNumeros.Services;

namespace WebApiListaNumeros.Controllers
{
    
    [Route("/v1/minipompom/basic")]
    [ApiController]
    public class BasicAuthController : ControllerBase
    {
        [HttpGet]
        [Route("exception")]
        public async Task<IActionResult> ThrowException()
        {
            throw new Exception("test");
        }

        //[ServiceFilter(typeof(BasicAuthorizationFilter))]

        // TODO: probar esto
        //[Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetResult()
        {
            try
            {
                var content = "BasicAuth API";
                return Ok(content);
            }
            catch
            {
                return BadRequest();
            }
        }


    }
    #region JsonProperties  
    /// <summary>  
    /// Json Properties  
    /// </summary>  
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
    #endregion
}
