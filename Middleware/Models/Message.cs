using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Middlewares.Models
{
    public class Message
    {
        public string title { get; set; }
        public object detail { get; set; }
    }

}
