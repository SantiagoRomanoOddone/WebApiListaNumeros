using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.SecurityDisponibilityHandler
{
    public interface ISecurityDisponibilityFilter
    {
        Task/*<HttpContext>*/ DisponibilityCheck(HttpContext context);
        Task/*<bool>/*<HttpContext>*/ SecurityCheck(HttpContext context);

    }
}
