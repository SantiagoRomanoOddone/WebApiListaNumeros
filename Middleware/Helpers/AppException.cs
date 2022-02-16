using System;
using System.Globalization;


namespace Middlewares.Helpers
{
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message) : base(message) { }

    }
}
