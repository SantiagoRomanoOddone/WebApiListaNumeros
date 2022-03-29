using Microsoft.AspNetCore.Http;
using Middlewares.Models;
using System.Threading.Tasks;

namespace Middlewares.SecurityDisponibilityHandler
{
    public interface ISecurityFilter
    {       
        Task SecurityCheckAsync(HttpContext context);
    }
}
