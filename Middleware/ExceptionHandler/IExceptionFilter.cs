﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.ExceptionHandler
{
    public interface IExceptionFilter
    {
        Task SetStatusCode(HttpContext context, Exception ex);
    }
}
