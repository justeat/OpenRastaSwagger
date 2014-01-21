namespace OpenRastaSwagger.Discovery
{
    public class ResponseCode
    {
        public int StatusCode { get; set; }
        public string Description { get; set; }

        public ResponseCode(int statusCode, string description)
        {
            StatusCode = statusCode;
            Description = description;
        }
    }
}