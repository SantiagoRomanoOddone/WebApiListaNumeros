using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.FunctionalityHandler
{
    public interface IFunctionalityFilter
    {
        Task FunctionalityCheck(HttpContext context);
    }
}
