using System;
using System.Net;
using System.Text;

namespace Health.CovidKhPublisher.Communication.Exceptions
{
    public class CommunicationException : Exception
    {
        private const string DefaultErrorMessage = "An error occurred while communicating with the remote host.";

        public CommunicationException() : this(DefaultErrorMessage)
        {
        }

        public CommunicationException(string message) : base(message)
        {
        }

        public CommunicationException(HttpStatusCode? statusCode, string requestUri, string requestContent = null, string responseContent = null)
            : this(BuildDetailedMessage(statusCode, requestUri, requestContent, responseContent), statusCode, requestUri, requestContent, responseContent)
        {
        }

        public CommunicationException(string message, HttpStatusCode? statusCode, string requestUri, string requestContent = null, string responseContent = null)
            : base(message)
        {
            StatusCode = statusCode;
            RequestUri = requestUri;
            RequestContent = requestContent;
            ResponseContent = responseContent;
        }

        public CommunicationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CommunicationException(string message, HttpStatusCode? statusCode, string requestUri, Exception innerException, string requestContent = null, string responseContent = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            RequestUri = requestUri;
            RequestContent = requestContent;
            ResponseContent = responseContent;
        }

        public HttpStatusCode? StatusCode { get; }

        public string RequestUri { get; }

        public string RequestContent { get; }

        public string ResponseContent { get; }

        private static string BuildDetailedMessage(HttpStatusCode? statusCode, string requestUri, string requestContent = null, string responseContent = null)
        {
            var statusCodeString = statusCode.HasValue
                ? $"{(int)statusCode} {statusCode}"
                : string.Empty;

            var formattedMessage = new StringBuilder($"{DefaultErrorMessage}")
                .AppendLine()
                .AppendLine(@$"{nameof(StatusCode)}=""{statusCodeString}"",")
                .AppendLine(@$"{nameof(RequestUri)}=""{requestUri}"",")
                .AppendLine(@$"{nameof(RequestContent)}=""{requestContent}"",")
                .AppendLine(@$"{nameof(ResponseContent)}=""{responseContent}""");

            return formattedMessage.ToString();
        }
    }
}
