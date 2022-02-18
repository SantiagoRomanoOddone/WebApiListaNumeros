using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.ExceptionHandler
{
    public class AppException : Exception
    {
        // custom exception class for throwing application specific exceptions 
        // that can be caught and handled within the application
        public AppException() : base() { }

        public AppException(string message) : base(message) { }
    }
}
