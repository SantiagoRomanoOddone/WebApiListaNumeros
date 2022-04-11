
namespace Middlewares.Auxiliaries
{
    public static class Constant
    {      
        
        public const string ISSUER = "https://localhost:44393";
        public const string AUDIENCE = "https://localhost:44388";
        public const string OPENTELEMETRY_SOURCE = "miniPOMPOM";

        public const string RESPONSE_CONTENT_TYPE = "application/json";

        public const string CACHE_REFRESH_TIME = "00:10";

        public const string TRACE_ID_BAGGAGE = "traceId";

        public static class Bearer
        {
            public const string KEY = "SecretKeywqewqeqqqqqqqqqqqweeeeeeeeeeeeeeeeeeeqweqe";
            public const string CHACHEKEYNAME = "CacheKeyBearer";
            public const string CHACHEKEYTIME = "CacheTimeBearer";
        }

        public static class Basic
        {
            public const string USER = "Admin";
            public const string PASSWORD = "Admin123";
            public const string CHACHEKEYNAME = "CacheKeyBasic";
            public const string CHACHEKEYTIME = "CacheTimeBasic";

        }

    }
}
