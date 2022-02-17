using System;
using System.Globalization;


namespace Middlewares.Helpers
{
    public class AppException : Exception
    {
        // custom exception class for throwing application specific exceptions 
        // that can be caught and handled within the application
        public AppException() : base() { }

        public AppException(string message) : base(message) { }

    }
}
