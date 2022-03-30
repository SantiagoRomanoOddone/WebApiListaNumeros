using System;
using System.Threading.Tasks;

namespace Middlewares.ExceptionHandler
{
    public interface IExceptionFilter
    {
        Task SetStatusCodeAsync(Exception ex);
    }
}
