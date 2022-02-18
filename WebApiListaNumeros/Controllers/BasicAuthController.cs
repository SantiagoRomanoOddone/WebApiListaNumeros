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
using WebApiListaNumeros.Services;

namespace WebApiListaNumeros.Controllers
{
    
    [Route("/v1/minipompom/basic")]
    [ApiController]
    public class BasicAuthController : ControllerBase
    {

        [HttpGet("list")]
        public async Task<IActionResult> GetResults()
        {
           
            try
            {
                var content = "BasicAuth API";
                return Ok(content);
            }
            catch
            {
                throw new AppException("Email or password is incorrect");
                //return response.StatusCode = (int)HttpStatusCode.BadRequest;
                //return BadRequest();
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
