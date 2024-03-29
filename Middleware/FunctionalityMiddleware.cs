﻿using Microsoft.AspNetCore.Http;
using Middlewares.FunctionalityHandler;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Middlewares
{
    public class FunctionalityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICacheProvider _cacheProvider;

        public FunctionalityMiddleware(RequestDelegate next, ICacheProvider cacheProvider)
        {
            _next = next;
            _cacheProvider = cacheProvider;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _cacheProvider.FunctionalityCheckAsync();

                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }                  
        }

    }
}
