using System;
using Health.CovidKhPublisher.Business.Models.Domain;

namespace Health.CovidKhPublisher.Business.Models.Dto
{
    public class AggregatedReportDto
    {
        public DateTime Date { get; set; }

        public Location Location { get; set; }

        public int Confirmed { get; set; }

        public int Deaths { get; set; }

        public int Recovered { get; set; }

        public int Existing { get; set; }

        public int DeltaConfirmed { get; set; }

        public int DeltaDeaths { get; set; }

        public int DeltaRecovered { get; set; }

        public int DeltaExisting { get; set; }

        public AverageGrowthDto AverageGrowthOfConfirmed { get; set; }

        public MaxGrowthDto MaxGrowthOfConfirmed { get; set; }
    }

    public class AverageGrowthDto
    {
        public int CurrentWeekDays { get; set; }

        public double CurrentWeek { get; set; }

        public double PreviousWeek { get; set; }

        public double TwoWeeksAgo { get; set; }

        public double MonthAgo { get; set; }
    }

    public class MaxGrowthDto
    {
        public int Value { get; set; }

        public DateTime Date { get; set; }
    }
}
