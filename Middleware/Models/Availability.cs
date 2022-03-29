using System.Collections.Generic;

namespace Middlewares.Models
{
    public class Availability
    {
        public BusinessHours business_hours { get; set; }
        public List<object> out_of_service_list { get; set; }
    }
}
