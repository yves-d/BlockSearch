namespace BlockSearch.Infrastructure.Options
{
    public abstract class HttpClientOptions
    {
        public string BaseUri { get; set; }
        public string ProjectId { get; set; }
    }
}
