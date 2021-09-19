using System.Net;

namespace BlockSearch.Application.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
    }
}
