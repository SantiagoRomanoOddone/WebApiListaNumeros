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
        Task DisponibilityCheck(HttpContext context);
        Task SecurityCheck(HttpContext context);

    }
}
