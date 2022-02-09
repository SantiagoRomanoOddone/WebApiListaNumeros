using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiListaNumeros.Controllers;

namespace WebApiListaNumeros.Services
{
    public interface IUserService
    {
        bool IsValidUserInformation(LoginModel model);
        
        LoginModel GetUserDetails();
    }
}
