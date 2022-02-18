using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.SecurityDisponibilityHandler
{
    public interface ISecurityDisponibilityFilter
    {
        Task<HttpContext> DisponibilityCheck(HttpContext context);
        Task<HttpContext> SecurityCheck(HttpContext context);

    }
}
