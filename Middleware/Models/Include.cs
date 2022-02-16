using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Middlewares.Models
{
    public class Include
    {
        public string weekday { get; set; }
        public string from_hour { get; set; }
        public string to_hour { get; set; }
    }
}
