using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LoginApp.Components.Enums.Api;

namespace LoginApp.Components
{
    public class ApiResponse
    {
        public bool hasError { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic data { get; set; }
        public int statusCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorMessage { get; set; }

        public ApiResponse(dynamic resposeData)
        {
            hasError = false;
            data = resposeData;
            statusCode = 200;
            errorMessage = null;
        }

        public ApiResponse(Error error)
        {
            hasError = true;
            statusCode = (int)error;
            errorMessage = error.ToString();
        }
    }
}
