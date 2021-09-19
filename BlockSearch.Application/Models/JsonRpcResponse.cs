using Newtonsoft.Json;

namespace BlockSearch.Application.Models
{
    public class JsonRpcResponse
    {
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("result")]
        public object Result { get; set; }
    }
}
