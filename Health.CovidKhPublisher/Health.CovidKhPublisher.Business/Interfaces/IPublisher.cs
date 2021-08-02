using System.Threading.Tasks;
using Health.CovidKhPublisher.Business.Models.Domain;

namespace Health.CovidKhPublisher.Business.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync(Message message);
    }
}
