using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Middlewares.FunctionalityHandler
{
    public interface ICacheProvider
    {
        Task FunctionalityCheckAsync(HttpContext context);
    }
}
