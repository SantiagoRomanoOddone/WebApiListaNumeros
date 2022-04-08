using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using OpenTelemetry;

namespace Telemetry
{   
    public class BaggageInfo
    {    
        public static void EnrichBaggage(IHttpContextAccessor httpContextAccessor, Activity activity)
        {
            //Baggage will flow to child activities.
            var HttpContext = httpContextAccessor.HttpContext;
        
            
            Baggage.Current.SetBaggage("ConnectionId", HttpContext.Connection.Id);
            Baggage.Current.SetBaggage("TraceIdentifier", HttpContext.TraceIdentifier);


            activity?.SetTag("ConnectionIdInfo", Baggage.Current.GetBaggage("ConnectionId"));
            activity?.SetTag("TraceIdentifier", Baggage.Current.GetBaggage("TraceIdentifier"));

        }
    }
}
