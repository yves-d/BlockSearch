using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlockSearch.Application.Models
{
    public class JsonRpcRequest
    {
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; private set; }

        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public List<object> Params { get; set; }

        public JsonRpcRequest()
        {
            JsonRpc = "2.0";
            Id = 1;
        }
    }
}
