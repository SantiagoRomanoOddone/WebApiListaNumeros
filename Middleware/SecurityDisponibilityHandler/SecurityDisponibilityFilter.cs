using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Middlewares.ExceptionHandler;
using Middlewares.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.SecurityDisponibilityHandler
{
    public class SecurityDisponibilityFilter : ISecurityDisponibilityFilter
    {
        public async Task<HttpContext> DisponibilityCheck(HttpContext context)
        {
            Root response = JsonConvert.DeserializeObject<Root>(context.Items["functionality-response"].ToString());
            bool available = false;
            string day = DateTime.Now.DayOfWeek.ToString().ToLower().Substring(0, 3);
            TimeSpan now = DateTime.Now.TimeOfDay;

            try
            {
                Include include = response.data.availability.business_hours.includes.Find(x => x.weekday == day);
                TimeSpan fromHour = Convert.ToDateTime(include.from_hour).TimeOfDay;
                TimeSpan toHour = Convert.ToDateTime(include.to_hour).TimeOfDay;

                if (include != null && fromHour < now && toHour > now)
                {
                    available = true;
                }
            }
            catch
            {
                available = false;
            }
            if (available == false)
            {
                throw new UnauthorizedAccessException("Unauthorized! Only Weekdays from 8 am to 10 pm");
            }
            return context;

        }

        public async Task<HttpContext> SecurityCheck(HttpContext context)
        {
            throw new UnauthorizedAccessException("Unauthorized! Only Weekdays from 8 am to 10 pm")
        }
    }
}
