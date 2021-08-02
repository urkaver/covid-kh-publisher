using System;
using System.Threading.Tasks;
using Health.CovidKhPublisher.Business.Models.Domain;
using Health.CovidKhPublisher.Business.Models.Dto;

namespace Health.CovidKhPublisher.Business.Interfaces
{
    public interface IReportService
    {
        Task<AggregatedReportDto> GetByDateAsync(Location location, DateTime now);
    }
}
