using System.Net.Http;
using System.Threading.Tasks;
using Health.CovidKhPublisher.Communication.Exceptions;
using Health.CovidMonitor.Core.Communication.Extensions;

namespace Health.CovidKhPublisher.Communication.Helpers
{
    public sealed class HttpResponseMessageValidator
    {
        public async Task ValidateResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            if (response.IsClientErrorStatusCode() || response.IsServerErrorStatusCode())
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                throw new CommunicationException(
                    response.StatusCode,
                    response.RequestMessage.RequestUri.ToString(),
                    null,
                    responseContent);
            }

            throw new CommunicationException(
                response.StatusCode,
                response.RequestMessage.RequestUri.ToString());
        }
    }
}
