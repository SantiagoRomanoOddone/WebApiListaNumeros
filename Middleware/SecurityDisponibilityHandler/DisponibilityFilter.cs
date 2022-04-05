using Middlewares.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Middlewares.SecurityDisponibilityHandler
{
    public class DisponibilityFilter : IDisponibilityFilter
    {
        private static readonly ActivitySource Activity = new("miniPOMPOM");

        public async Task DisponibilityCheckAsync(Root response)
        {
            using var activity = Activity.StartActivity("In Disponibility Filter");

            string day = DateTime.Now.DayOfWeek.ToString().ToLower().Substring(0, 3);
            TimeSpan now = DateTime.Now.TimeOfDay;
            Include include = response.data.availability.business_hours.includes.Find(x => x.weekday == day);
            if (include != null && now < Convert.ToDateTime(include.from_hour).TimeOfDay || now > Convert.ToDateTime(include.to_hour).TimeOfDay)
            {
                throw new UnauthorizedAccessException("Unauthorized! Only Weekdays from 8 am to 10 pm");
            }
        }
    }
}
