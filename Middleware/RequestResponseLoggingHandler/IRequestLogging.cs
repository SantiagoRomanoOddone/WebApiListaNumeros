
using System.Threading.Tasks;

namespace Middlewares.RequestResponseLoggingHandler
{
    public interface IRequestLogging
    {
        Task LogRequest();
    }
}
