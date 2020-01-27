using Newtonsoft.Json;

namespace DevOpsSync.WebApp.API.Models.GitHub
{
    public class Token
    {
        public class Request
        {
            [JsonProperty(PropertyName = "client_id")]
            public string ClientId { get; set; }

            [JsonProperty(PropertyName = "client_secret")]
            public string CilentSecret { get; set; }

            public string Code { get; set; }
        }

        public class Response
        {
            [JsonProperty(PropertyName = "access_token")]
            public string AccessToken { get; set; }

            [JsonProperty(PropertyName = "token_type")]
            public string TokenType { get; set; }

            public string Scope { get; set; }
        }
    }
}
