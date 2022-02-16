using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Middlewares.Models
{
    public class BusinessHours
    {
        public bool includes_holidays { get; set; }
        public List<Include> includes { get; set; }
        public Message message { get; set; }
    }
}
