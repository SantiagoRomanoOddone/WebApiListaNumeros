using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.RequestResponseModels
{
    public class ResponseInformation
    {
        public string traceId { get; set; }
        public string http_response_body { get; set; }
        public string http_response_headers { get; set; }
        public string http_response_status_code { get; set; }
    }
}
