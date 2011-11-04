namespace Kigg.Infrastructure
{
    public class ThumbnailSettings
    {
        public ThumbnailSettings(string apiKey, string endpoint)
        {
            Check.Argument.IsNotNullOrEmpty(apiKey, "apiKey");
            Check.Argument.IsNotNullOrEmpty(endpoint, "endPoint");

            ApiKey = apiKey;
            Endpoint = endpoint;
        }

        public string ApiKey
        {
            get;
            private set;
        }

        public string Endpoint
        {
            get;
            private set;
        }
    }
}