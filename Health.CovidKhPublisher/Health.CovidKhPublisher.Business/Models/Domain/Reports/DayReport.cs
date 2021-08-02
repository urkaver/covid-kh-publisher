using System;

namespace Health.CovidKhPublisher.Business.Models.Domain.Reports
{
    public class DayReport
    {
        public DateTime Date { get; set; }

        public Location Location { get; set; }

        public int Confirmed { get; set; }

        public int Deaths { get; set; }

        public int Recovered { get; set; }

        public int Existing { get; set; }
    }
}
