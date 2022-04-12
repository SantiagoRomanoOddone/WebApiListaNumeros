using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OpenTelemetry;
using Telemetry.Auxiliaries;

namespace Telemetry
{   
    public class BaggageInfo
    {
        public static async Task EnrichBaggage(HttpContext context, Activity activity)
        {
            SetBaggages(context);

            SetSpecificTags(activity);

        }
        public static void SetBaggages(HttpContext context)
        {
            Baggage.Current.SetBaggage(Constant.TRACE_ID_BAGGAGE, context.TraceIdentifier);
        }
        public static async Task SetSpecificTags(Activity activity)
        {
            activity?.SetTag(Constant.TRACE_ID_BAGGAGE, Baggage.Current.GetBaggage(Constant.TRACE_ID_BAGGAGE));
        }
    }
}
