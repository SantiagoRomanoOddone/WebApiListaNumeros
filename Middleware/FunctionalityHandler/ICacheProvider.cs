using System.Threading.Tasks;

namespace Middlewares.FunctionalityHandler
{
    public interface ICacheProvider
    {
        Task FunctionalityCheckAsync();
    }
}
