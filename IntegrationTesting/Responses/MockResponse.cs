using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTesting.Responses
{
    public class MockResponse
    {
        public static class SecurityResponse
        {
            public const string RESPONSE_BASIC_OK = "QWRtaW46QWRtaW4xMjM=";
            public const string RESPONSE_BASIC_NOTOK = "QWRtaTpBZG1pbjEy";
            public const string RESPONSE_BEARER_NOTOK = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IkFkbWluIiwibmJmIjoxNjQ1ODExMDQ4LCJleHAiOjE2NDU4MTExMDgsImlhdCI6MTY0NTgxMTA0OCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTMiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4OCJ9.rRUEChrSQOTsgdrrVI5JjZW7_P-O8XpkRUn7WqzFz8k";
            public const string RESPONSE_BEARER_OK = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IkFkbWluIiwiaW5wdXQtYm9keSI6IntcIm1ldGhvZFwiOlwiUE9TVFwiLFwiY2hhbm5lbFwiOlwic3VjdXJzYWxcIixcInBhdGhcIjpcIi92MS9taW5pcG9tcG9tL2p3dC9jcmVhdGlvbi9BdXRoXCJ9IiwibmJmIjoxNjQ2OTIzOTAxLCJleHAiOjE2NDY5MzgzMDEsImlhdCI6MTY0NjkyMzkwMSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTMiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4OCJ9.qfXLqaQpdSwfCKFBfV3Fldf1cIh4CG8KjP8-Y9ctCHg";

        }
    }
}
