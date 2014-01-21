namespace CloudSurvey.Helpers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public static class ApiControllerExtensions
    {
        public static HttpResponseException WebException(this ApiController controller, HttpStatusCode code, string message = "")
        {
            var responseMessage = new HttpResponseMessage(code);
            if (!string.IsNullOrWhiteSpace(message))
            {
                responseMessage.Content = new StringContent(message);
            }

            return new HttpResponseException(responseMessage);
        }
    }
}