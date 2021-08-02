using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Health.CovidKhPublisher.Business.Interfaces;
using Health.CovidKhPublisher.Business.Models.Domain;
using Health.CovidKhPublisher.Business.Models.Domain.Reports;
using Health.CovidKhPublisher.Communication.Interfaces;
using Health.CovidKhPublisher.Communication.Models;

namespace Health.CovidKhPublisher.Communication
{
    public class RnboReportProvider : IRnboReportProvider
    {
        private readonly IHttpCommunicationClient _communicationClient;

        public RnboReportProvider(IHttpCommunicationClient communicationClient)
        {
            _communicationClient = communicationClient;
        }

        public async Task<DayReport[]> GetAllDayReports(Location location)
        {
            var response = await _communicationClient.DoGetAsync<RnboMainDataApiResponse>(RnboApiUrlBuilder.BuildMainDataUrl(location));

            var dayReports = new Dictionary<string, DayReport>();
            for (int index = 0; index < response.Dates.Length; index++)
            {
                string key = response.Dates[index];

                var reportEntry = new DayReport
                {
                    Date = DateTime.ParseExact(key, "yyyy-MM-dd", new DateTimeFormatInfo()),
                    Location = location,
                    Confirmed = response.Confirmed[index],
                    Deaths = response.Deadths[index],
                    Existing = response.Existing[index],
                    Recovered = response.Recovered[index]
                };

                dayReports.Add(key, reportEntry);
            }

            return dayReports.Select(_ => _.Value).ToArray();
        }
    }
}
