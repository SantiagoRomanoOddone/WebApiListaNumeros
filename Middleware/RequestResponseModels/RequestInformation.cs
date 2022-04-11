using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares.RequestResponseModels
{
    public class RequestInformation
    {
        public string traceId { get; set; }
        public string http_schema { get; set; }
        public string http_host { get; set; }
        public string http_request_path { get; set; }
        public string http_request_method { get; set; }
        public string http_request_headers { get; set; }
        public string http_request_body { get; set; }
        public string http_request_query_string { get; set; }

    }
}
