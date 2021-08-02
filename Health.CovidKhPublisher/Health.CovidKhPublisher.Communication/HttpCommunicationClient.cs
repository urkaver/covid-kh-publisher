using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Health.CovidKhPublisher.Communication.Helpers;
using Health.CovidKhPublisher.Communication.Interfaces;
using Microsoft.Extensions.Logging;

namespace Health.CovidKhPublisher.Communication
{
    public class HttpCommunicationClient : IHttpCommunicationClient
    {
        private readonly ILogger<HttpCommunicationClient> _logger;
        private readonly HttpClient _httpClient;
        private readonly HttpResponseMessageValidator _validator;
        private readonly HttpContentJsonSerializer _serializer;

        public HttpCommunicationClient(HttpClient httpClient, ILogger<HttpCommunicationClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _serializer = new HttpContentJsonSerializer();
            _validator = new HttpResponseMessageValidator();
        }

        public async Task<TResult> DoGetAsync<TResult>(string uri, IEnumerable<(string, string)> headers = null)
        {
            _logger.LogInformation($"Sending HTTP request {HttpMethod.Get} {uri}");

            var request = GetRequestMessage(HttpMethod.Get, uri, headers);

            var response = await _httpClient.SendAsync(request);

            await _validator.ValidateResponse(response);

            var contentStream = await response.Content.ReadAsStreamAsync();

            return _serializer.DeserializeJsonFromStream<TResult>(contentStream);
        }

        private HttpRequestMessage GetRequestMessage(HttpMethod httpMethod, string uri, IEnumerable<(string, string)> headers = null)
        {
            var requestMessage = new HttpRequestMessage(httpMethod, uri);

            if (headers != null && headers.Any())
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.Add(header.Item1, header.Item2);
                }
            }

            return requestMessage;
        }
    }
}
