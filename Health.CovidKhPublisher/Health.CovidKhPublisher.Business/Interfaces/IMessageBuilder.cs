using Health.CovidKhPublisher.Business.Models.Domain;

namespace Health.CovidKhPublisher.Business.Interfaces
{
    public interface IMessageBuilder<in T> where T : class
    {
        Message Build(T report);
    }
}
