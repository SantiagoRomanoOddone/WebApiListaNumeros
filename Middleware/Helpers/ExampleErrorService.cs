using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.Helpers
{
    public class ExampleErrorService
    {
        public void ExampleErrors()
        {
            // a custom app exception that will return a 400 response
            throw new AppException("Email or password is incorrect");

            // a key not found exception that will return a 404 response
            throw new KeyNotFoundException("Account not found");

            throw new UnauthorizedAccessException("Unauthorized!");

            throw new Exception("Something Went Wrong! Error Ocurred");
        }
    }
}
