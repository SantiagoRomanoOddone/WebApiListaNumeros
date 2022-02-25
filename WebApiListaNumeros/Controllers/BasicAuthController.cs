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
    [Route("/v1/minipompom/basic")]
    [ApiController]
    public class BasicAuthController : ControllerBase
    {
        [HttpGet("list")]
        public IActionResult GetResult()
        {
            return Ok(" BasicAuth API");
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
