using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Health.CovidKhPublisher.Business.Extensions;
using Health.CovidKhPublisher.Business.Models.Domain.Reports;

namespace Health.CovidKhPublisher.Business.Models.Domain.Collections
{
    public class DayReportSet : IEnumerable<DayReport>
    {
        private readonly Dictionary<DateTime, DayReport> _dayReportDictionary;

        public DayReportSet(IEnumerable<DayReport> dayReports)
        {
            if (dayReports == null)
            {
                throw new ArgumentNullException(nameof(dayReports));
            }

            _dayReportDictionary = dayReports.OrderBy(_ => _.Date.Date).ToDictionary(_ => _.Date.Date, _ => _);
        }

        public DayReport this[DateTime date] => _dayReportDictionary.ContainsKey(date.Date) ? _dayReportDictionary[date.Date] : null;

        public DayReport Min => _dayReportDictionary.First().Value;

        public DateTime MinDate => _dayReportDictionary.First().Key;

        public DayReport Max => _dayReportDictionary.Last().Value;

        public DateTime MaxDate => _dayReportDictionary.Last().Key;

        public int Length => _dayReportDictionary.Count;
        
        public DayReportSet Truncate(DateTime start, DateTime end)
        {
            if (end < start)
            {
                throw new InvalidOperationException("The start date should be less than end date.");
            }

            return new DayReportSet(GetRange(start, end));
        }

        public IEnumerator<DayReport> GetEnumerator()
        {
            foreach (var dayReport in _dayReportDictionary.Values)
                yield return dayReport;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<DayReport> GetRange(DateTime start, DateTime end)
        {
            foreach (DateTime day in DateTimeExtensions.EachDay(start, end))
            {
                var value = this[day.Date];

                if (value == null)
                    continue;

                yield return value;
            }
        }
    }
}
