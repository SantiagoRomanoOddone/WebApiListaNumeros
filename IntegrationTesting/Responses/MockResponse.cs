using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTesting.Responses
{
    public class MockResponse
    {
        private readonly IHttpClientFactory _clientFactory;
        public MockResponse(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public static class SecurityResponse
        {
            public const string RESPONSE_BASIC_OK = "QWRtaW46QWRtaW4xMjM=";
            public const string RESPONSE_BASIC_NOTOK = "QWRtaTpBZG1pbjEy";
            public const string RESPONSE_BEARER_NOTOK = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IkFkbWluIiwibmJmIjoxNjQ1ODExMDQ4LCJleHAiOjE2NDU4MTExMDgsImlhdCI6MTY0NTgxMTA0OCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTMiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4OCJ9.rRUEChrSQOTsgdrrVI5JjZW7_P-O8XpkRUn7WqzFz8k";
            public const string RESPONSE_BEARER_NOTOK_WRONG_SIGNATURE = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            public const string RESPONSE_BEARER_NOTOK_ONE_ARGUMENT = "HOLA";
            public const string RESPONSE_BEARER_NOTOK_WRONGHEADER = "a.eyJpZCI6IkFkbWluIiwiaW5wdXQtYm9keSI6IntcIm1ldGhvZFwiOlwiUE9TVFwiLFwiY2hhbm5lbFwiOlwic3VjdXJzYWxcIixcInBhdGhcIjpcIi92MS9taW5pcG9tcG9tL2p3dC9BdXRoXCJ9IiwibmJmIjoxNjQ3NDM3OTYwLCJleHAiOjE2NDc0ODExNjAsImlhdCI6MTY0NzQzNzk2MCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTMiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4OCJ9.RGlAsU4Sxu8PN2-FlNJ_8Zfu_EkXJXDYdGYM1d48VyA";
            public const string RESPONSE_BEARER_NOTOK_NOSIGNATURE = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IkFkbWluIiwiaW5wdXQtYm9keSI6IntcIm1ldGhvZFwiOlwiUE9TVFwiLFwiY2hhbm5lbFwiOlwic3VjdXJzYWxcIixcInBhdGhcIjpcIi92MS9taW5pcG9tcG9tL2p3dC9BdXRoXCJ9IiwibmJmIjoxNjQ3NDM3OTYwLCJleHAiOjE2NDc0ODExNjAsImlhdCI6MTY0NzQzNzk2MCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTMiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4OCJ9.";
            public const string RESPONSE_BEARER_NOTOK_WRONGPAYLOAD = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.a.RGlAsU4Sxu8PN2-FlNJ_8Zfu_EkXJXDYdGYM1d48VyA";
            public const string RESPONSE_BEARER_OK = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IkFkbWluIiwiaW5wdXQtYm9keSI6IntcIm1ldGhvZFwiOlwiUE9TVFwiLFwiY2hhbm5lbFwiOlwic3VjdXJzYWxcIixcInBhdGhcIjpcIi92MS9taW5pcG9tcG9tL2p3dC9BdXRoXCJ9IiwibmJmIjoxNjQ4NTYwMDQ0LCJleHAiOjE2NDg2MDMyNDQsImlhdCI6MTY0ODU2MDA0NCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTMiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4OCJ9._YHyp3_W44jYREp7eiFUbQNXo8ij0A77mp3A8LePAE8";
        }
    }
}
