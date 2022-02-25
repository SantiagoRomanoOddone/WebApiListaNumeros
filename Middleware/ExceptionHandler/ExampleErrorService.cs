using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.ExceptionHandler
{
    public class ExampleErrorService
    {
        public void ExampleErrors()
        {
            throw new AppException("Email or password is incorrect");
            throw new KeyNotFoundException("Account not found");
            throw new UnauthorizedAccessException("Unauthorized!");
            throw new Exception("Something Went Wrong! Error Ocurred");
        }
    }
}
