using System.Threading.Tasks;
using Health.CovidKhPublisher.Business.Models.Domain;
using Health.CovidKhPublisher.Business.Models.Domain.Reports;

namespace Health.CovidKhPublisher.Business.Interfaces
{
    public interface IRnboReportProvider
    {
        Task<DayReport[]> GetAllDayReports(Location location);
    }
}
