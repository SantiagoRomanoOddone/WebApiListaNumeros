using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using OpenTelemetry;
using Telemetry.Auxiliaries;

namespace Telemetry
{   
    public class BaggageInfo
    {
        public static void SetBaggages(HttpContext context)
        {
            Baggage.Current.SetBaggage(Constant.TRACE_ID_BAGGAGE, context.TraceIdentifier);
        }
        public static void EnrichBaggage(Activity activity)
        {
            activity?.SetTag(Constant.TRACE_ID_BAGGAGE, Baggage.Current.GetBaggage(Constant.TRACE_ID_BAGGAGE));
        }
    }
}
