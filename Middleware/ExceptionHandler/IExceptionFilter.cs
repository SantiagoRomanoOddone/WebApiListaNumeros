using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Middlewares.ExceptionHandler
{
    public interface IExceptionFilter
    {
        Task SetStatusCodeAsync(HttpContext context, Exception ex);
    }
}
