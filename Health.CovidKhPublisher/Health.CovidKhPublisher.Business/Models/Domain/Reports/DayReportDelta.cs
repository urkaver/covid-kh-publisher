namespace Health.CovidKhPublisher.Business.Models.Domain.Reports
{
    public class DayReportDelta
    {
        public DayReport First { get; private set; }

        public DayReport Second { get; private set; }

        public int DeltaConfirmed { get; private set; } = 0;

        public int DeltaDeaths { get; private set; } = 0;

        public int DeltaRecovered { get; private set; } = 0;

        public int DeltaExisting { get; private set; } = 0;

        public static DayReportDelta Between(DayReport first, DayReport second)
        {
            return new DayReportDelta
            {
                First = first,
                Second = second,
                DeltaConfirmed = first == null ? 0 : second.Confirmed - first.Confirmed,
                DeltaDeaths = first == null ? 0 : second.Deaths - first.Deaths,
                DeltaExisting = first == null ? 0 : second.Existing - first.Existing,
                DeltaRecovered = first == null ? 0 : second.Recovered - first.Recovered
            };
        }
    }
}
