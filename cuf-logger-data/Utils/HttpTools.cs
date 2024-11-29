using System;
using cuf_admision_data.Configuration;
using RestSharp;

namespace cuf_admision_data.Utils
{
    public static class HttpTools
    {
        public static void SetAdmisionHeaders(
            ref RestRequest request, string processName, string token, string uid = null)
        {
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("x-api-key", token);
            request.AddHeader("source.process_name", processName);
            request.AddHeader("source.process_uid", uid ?? Guid.NewGuid().ToString());
            request.AddHeader("source.channel_id", "02");
            request.AddHeader("source.consumer_id", "1002");
            request.AddHeader("source.user_id", "AdmisionService");
            request.AddHeader("attempt_counter", "1");
        }
    }
}

