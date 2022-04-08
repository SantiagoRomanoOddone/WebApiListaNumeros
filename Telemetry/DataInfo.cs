using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Telemetry
{
    public static class DataInfo
    {
        public static void Enrich(Activity activity, string eventName, object obj)
        {
           
            if (obj is HttpRequest request)
            {
                var context = request.HttpContext;
                activity.AddTag("http.flavor", GetHttpFlavour(request.Protocol));
                activity.AddTag("http.scheme", request.Scheme);
                activity.AddTag("http.client_ip", context.Connection.RemoteIpAddress);
                activity.AddTag("http.request_content_length", request.ContentLength);
                activity.AddTag("http.request_content_type", request.ContentType);

                var user = context.User;
                if (user.Identity?.Name is not null)
                {
                    activity.AddTag("enduser.id", user.Identity.Name);
                    activity.AddTag(
                        "enduser.scope",
                        string.Join(',', user.Claims.Select(x => x.Value)));
                }
            }
            else if (obj is HttpResponse response)
            {
                activity.AddTag("http.response_content_length", response.ContentLength);
                activity.AddTag("http.response_content_type", response.ContentType);
            }

        }
        public static string GetHttpFlavour(string protocol)
        {
            if (HttpProtocol.IsHttp10(protocol))
            {
                return "1.0";
            }
            else if (HttpProtocol.IsHttp11(protocol))
            {
                return "1.1";
            }
            else if (HttpProtocol.IsHttp2(protocol))
            {
                return "2.0";
            }
            else if (HttpProtocol.IsHttp3(protocol))
            {
                return "3.0";
            }

            throw new InvalidOperationException($"Protocol {protocol} not recognised.");
        }

    }
}
