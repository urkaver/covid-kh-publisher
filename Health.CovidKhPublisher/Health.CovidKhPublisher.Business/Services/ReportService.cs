using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentDateTime;
using Health.CovidKhPublisher.Business.Extensions;
using Health.CovidKhPublisher.Business.Interfaces;
using Health.CovidKhPublisher.Business.Models.Domain;
using Health.CovidKhPublisher.Business.Models.Domain.Collections;
using Health.CovidKhPublisher.Business.Models.Domain.Reports;
using Health.CovidKhPublisher.Business.Models.Dto;

namespace Health.CovidKhPublisher.Business.Services
{
    public class ReportService : IReportService
    {
        private readonly IRnboReportProvider _rnboReportProvider;

        public ReportService(IRnboReportProvider rnboReportProvider)
        {
            _rnboReportProvider = rnboReportProvider;
        }

        public async Task<AggregatedReportDto> GetByDateAsync(Location location, DateTime now)
        {
            var dayReports = await _rnboReportProvider.GetAllDayReports(location);

            var reportSet = new DayReportSet(dayReports);

            var currentDayReport = reportSet[now];
            if (currentDayReport == null)
            {
                throw new InvalidOperationException($"Not found report for specified date '{now}' and location '{location}'.");
            }

            var previousDayDelta = reportSet.CalculateDeltaBetweenDays(now.PreviousDay(), now);
            var currentPeriodReportSet = reportSet.Truncate(reportSet.MinDate, now);

            var aggregatedReport = new AggregatedReportDto
            {
                Date = currentDayReport.Date,
                Location = currentDayReport.Location,
                Confirmed = currentDayReport.Confirmed,
                Deaths = currentDayReport.Deaths,
                Existing = currentDayReport.Existing,
                Recovered = currentDayReport.Recovered,
                DeltaConfirmed = previousDayDelta.DeltaConfirmed,
                DeltaDeaths = previousDayDelta.DeltaDeaths,
                DeltaExisting = previousDayDelta.DeltaExisting,
                DeltaRecovered = previousDayDelta.DeltaRecovered,
                AverageGrowthOfConfirmed = BuildAverageGrowthDto(currentPeriodReportSet),
                MaxGrowthOfConfirmed = BuildMaxGrowth(currentPeriodReportSet)
            };

            return aggregatedReport;
        }

        private AverageGrowthDto BuildAverageGrowthDto(DayReportSet reportSet)
        {
            DateTime now = reportSet.MaxDate;
            DateTime weekAgo = now.WeekEarlier();
            DateTime twoWeeksAgo = weekAgo.WeekEarlier();
            DateTime monthAgo = now.AddDays(-30);

            var currentWeekDeltaList = reportSet.CalculateDeltaForEachDay(now.BeginningOfWeek(), now);

            return new AverageGrowthDto
            {
                CurrentWeekDays = currentWeekDeltaList.Length,
                CurrentWeek = GetAverageGrowthOfConfirmed(currentWeekDeltaList),
                PreviousWeek = GetAverageGrowthOfConfirmed(reportSet.CalculateDeltaForEachDay(weekAgo.BeginningOfWeek(), weekAgo.EndOfWeek())),
                TwoWeeksAgo = GetAverageGrowthOfConfirmed(reportSet.CalculateDeltaForEachDay(twoWeeksAgo.BeginningOfWeek(), twoWeeksAgo.EndOfWeek())),
                MonthAgo = GetAverageGrowthOfConfirmed(reportSet.CalculateDeltaForEachDay(monthAgo.BeginningOfWeek(), monthAgo.EndOfWeek()))
            };
        }

        private double GetAverageGrowthOfConfirmed(DayReportDelta[] deltaList)
        {
            return Math.Round(deltaList.Sum(_ => _.DeltaConfirmed) * 1.0d / deltaList.Length, 1);
        }

        private MaxGrowthDto BuildMaxGrowth(DayReportSet reportSet)
        {
            var maxConfirmedDelta = reportSet.CalculateDeltaForEachDay()
                .Where(_ => _ != null)
                .OrderByDescending(_ => _.DeltaConfirmed)
                .ThenByDescending(_ => _.Second.Date)
                .FirstOrDefault();

            if (maxConfirmedDelta == null || maxConfirmedDelta.DeltaConfirmed == 0)
                return null;

            return new MaxGrowthDto
            {
                Date = maxConfirmedDelta.Second.Date,
                Value = maxConfirmedDelta.DeltaConfirmed
            };
        }
    }
}
