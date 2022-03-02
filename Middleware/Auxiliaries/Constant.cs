using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.Auxiliaries
{
    public static class Constant
    {      
        
        public const string ISSUER = "https://localhost:44393";
        public const string AUDIENCE = "https://localhost:44388";

        public static class Bearer
        {
            public const string KEY = "SecretKeywqewqeqqqqqqqqqqqweeeeeeeeeeeeeeeeeeeqweqe";
        }

        public static class Basic
        {
            public const string USER = "Admin";
            public const string PASSWORD = "Admin123";

        }

    }
}
