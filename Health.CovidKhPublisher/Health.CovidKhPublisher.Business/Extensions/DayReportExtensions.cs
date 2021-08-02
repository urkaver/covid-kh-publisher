using System;
using System.Linq;
using FluentDateTime;
using Health.CovidKhPublisher.Business.Models.Domain.Collections;
using Health.CovidKhPublisher.Business.Models.Domain.Reports;

namespace Health.CovidKhPublisher.Business.Extensions
{
    public static class DayReportExtensions
    {
        public static DayReportDelta[] CalculateDeltaForEachDay(this DayReportSet reportSet, DateTime? start = null, DateTime? end = null)
        {
            if (reportSet.Length == 0)
            {
                return null;
            }

            start ??= reportSet.MinDate;
            end ??= reportSet.MaxDate;

            if (end < start)
            {
                throw new InvalidOperationException("Start date should be less than end date.");
            }

            return DateTimeExtensions.EachDay(start.Value, end.Value).Select(date =>
                DayReportDelta.Between(reportSet[date.PreviousDay()], reportSet[date]))
                .ToArray();
        }

        public static DayReportDelta CalculateDeltaBetweenDays(this DayReportSet reportSet, DateTime start, DateTime end)
        {
            if (reportSet.Length == 0)
            {
                return null;
            }

            if (end < start)
            {
                throw new InvalidOperationException("Start date should be less than end date.");
            }

            return DayReportDelta.Between(reportSet[start], reportSet[end]);
        }
    }
}
