using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Middlewares.Models
{
    public class Availability
    {
        public BusinessHours business_hours { get; set; }
        public List<object> out_of_service_list { get; set; }
    }
}
