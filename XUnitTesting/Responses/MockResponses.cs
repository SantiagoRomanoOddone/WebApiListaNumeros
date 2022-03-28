﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTesting.Responses
{
    public class MockResponses
    {
        public static class FunctionalityResponse
        {
            public const string RESPONSE_OK = "{\n    \"data\": {\n        \"channel\": \"sucursal\",\n        \"endpoint\": \"/v1/minipompom/basic/list\",\n        \"method\": \"GET\",\n        \"availability\": {\n            \"business_hours\": {\n                \"includes_holidays\": true,\n                \"includes\": [\n                    {\n                        \"weekday\": \"mon\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"tue\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"wed\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"thu\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"fri\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"sat\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"sun\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    }\n                ],\n                \"message\": {\n                    \"title\": \"\",\n                    \"detail\": null\n                }\n            },\n            \"out_of_service_list\": []\n        },\n        \"config\": {\n            \"security\": {\n                \"scopelevel\":\"basic\"\n            }\n        }\n    }\n}";

            public const string RESPONSE_NOT_OK = "{\n    \"data\": {\n        \"channel\": \"sucursal\",\n        \"endpoint\": \"/v1/minipompom/basic/list\",\n        \"method\": \"GET\",\n        \"availability\": {\n            \"business_hours\": {\n                \"includes_holidays\": true,\n                \"includes\": [\n                    {\n                        \"weekday\": \"mon\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    },\n                    {\n                        \"weekday\": \"tue\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    },\n                    {\n                        \"weekday\": \"wed\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    },\n                    {\n                        \"weekday\": \"thu\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    },\n                    {\n                        \"weekday\": \"fri\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    },\n                    {\n                        \"weekday\": \"sat\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    },\n                    {\n                        \"weekday\": \"sun\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    }\n                ],\n                \"message\": {\n                    \"title\": \"\",\n                    \"detail\": null\n                }\n            },\n            \"out_of_service_list\": []\n        },\n        \"config\": {\n            \"security\": {\n                \"scopelevel\":\"basic\"\n            }\n        }\n    }\n}";
        }

        public static class SecurityResponse
        {
            public const string RESPONSE_BASIC_OK = "Basic QWRtaW46QWRtaW4xMjM=";
            public const string RESPONSE_BASIC_NOTOK = "Basic QWRtaTpBZG1pbjEy";
            public const string RESPONSE_BEARER_NOTOK = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IkFkbWluIiwibmJmIjoxNjQ1ODExMDQ4LCJleHAiOjE2NDU4MTExMDgsImlhdCI6MTY0NTgxMTA0OCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTMiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4OCJ9.rRUEChrSQOTsgdrrVI5JjZW7_P-O8XpkRUn7WqzFz8k";
            public const string RESPONSE_BEARER_OK = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IkFkbWluIiwiaW5wdXQtYm9keSI6IntcIm1ldGhvZFwiOlwiUE9TVFwiLFwiY2hhbm5lbFwiOlwic3VjdXJzYWxcIixcInBhdGhcIjpcIi92MS9taW5pcG9tcG9tL2p3dC9BdXRoXCJ9IiwibmJmIjoxNjQ4NDY5OTcxLCJleHAiOjE2NDg1MTMxNzEsImlhdCI6MTY0ODQ2OTk3MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTMiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4OCJ9.4c-EytDGggWnUdw0E8yzGnFH1hgBM3apwUZ42EbnJzE";

        }
    }
}
