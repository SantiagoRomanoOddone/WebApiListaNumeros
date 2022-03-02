using Microsoft.AspNetCore.Http;
using Middlewares.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.SecurityDisponibilityHandler
{
    public class DisponibilityFilter : IDisponibilityFilter
    {
        public async Task DisponibilityCheck(HttpContext context)
        {
            Root response = JsonConvert.DeserializeObject<Root>(context.Items["functionality-response"].ToString());
            string day = DateTime.Now.DayOfWeek.ToString().ToLower().Substring(0, 3);
            TimeSpan now = DateTime.Now.TimeOfDay;

            try
            {
                Include include = response.data.availability.business_hours.includes.Find(x => x.weekday == day);
                if (include != null && now < Convert.ToDateTime(include.from_hour).TimeOfDay || now > Convert.ToDateTime(include.to_hour).TimeOfDay )
                {
                    throw new UnauthorizedAccessException("Unauthorized! Only Weekdays from 8 am to 10 pm");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
