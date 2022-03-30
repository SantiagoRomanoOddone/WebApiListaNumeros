
using System.Threading.Tasks;
using Middlewares.Models;

namespace Middlewares.SecurityDisponibilityHandler
{
    public interface ISecurityFilter
    {       
        Task SecurityCheckAsync(Root response);
    }
}
