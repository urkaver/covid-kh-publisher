using System.Net.Http;

namespace Health.CovidMonitor.Core.Communication.Extensions
{
    internal static class HttpResponseMessageExtensions
    {
        internal static bool IsClientErrorStatusCode(this HttpResponseMessage responseMessage) => responseMessage != null
                                                                                                  && ((int)responseMessage.StatusCode >= 400)
                                                                                                  && ((int)responseMessage.StatusCode <= 499);

        internal static bool IsServerErrorStatusCode(this HttpResponseMessage responseMessage) => responseMessage != null
                                                                                                  && ((int)responseMessage.StatusCode >= 500)
                                                                                                  && ((int)responseMessage.StatusCode <= 599);
    }
}
