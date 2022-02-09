using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiListaNumeros.ErrorDetails
{
    public class ErrorInfo
    {
        public ErrorInfo()
        {
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; internal set; }
    }
}
