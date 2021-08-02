using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Health.CovidKhPublisher.Business.Models.Domain;

namespace Health.CovidKhPublisher.Business.Interfaces
{
    public interface IMessageService
    {
        Task SendDayReportMessageAsync(Location location, DateTime datetime);
    }
}
