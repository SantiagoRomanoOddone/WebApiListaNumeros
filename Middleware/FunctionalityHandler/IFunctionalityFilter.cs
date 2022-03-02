using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.FunctionalityHandler
{
    public interface IFunctionalityFilter
    {
        Task FunctionalityCheck(HttpContext context/*, IHttpClientFactory clientFactory, IMemoryCache memoryCache, IConfiguration configuration*/);
    }
}
