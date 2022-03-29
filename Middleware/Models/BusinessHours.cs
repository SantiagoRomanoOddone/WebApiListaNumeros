using System.Collections.Generic;

namespace Middlewares.Models
{
    public class BusinessHours
    {
        public bool includes_holidays { get; set; }
        public List<Include> includes { get; set; }
        public Message message { get; set; }
    }
}
