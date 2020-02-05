using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevOpsSync.WebApp.Services.Models.DevOps
{
    public class Token
    {
        public class Response
        {
            [JsonProperty(PropertyName = "access_token")]
            public string AccessToken { get; set; }

            [JsonProperty(PropertyName = "token_type")]
            public string TokenType { get; set; }

            [JsonProperty(PropertyName = "refresh_token")]
            public string RefreshToken { get; set; }

            public string Scope { get; set; }
        }
    }
}
