using System.Collections.Generic;
using System.Threading.Tasks;

namespace Health.CovidKhPublisher.Communication.Interfaces
{
    public interface IHttpCommunicationClient
    {
        Task<TResult> DoGetAsync<TResult>(string uri, IEnumerable<(string, string)> headers = null);
    }
}
