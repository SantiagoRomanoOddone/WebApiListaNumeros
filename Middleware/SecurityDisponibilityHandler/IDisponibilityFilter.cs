using System.Diagnostics;
using System.Threading.Tasks;
using Middlewares.Models;

namespace Middlewares.SecurityDisponibilityHandler
{
    public interface IDisponibilityFilter
    {
        Task DisponibilityCheckAsync(Root response);
    }
}
