using System;
using FluentDateTime;
using Health.CovidKhPublisher.Business.Interfaces;
using Health.CovidKhPublisher.Business.Models;
using Health.CovidKhPublisher.Business.Models.Domain;
using Health.CovidKhPublisher.Business.Models.Dto;

namespace Health.CovidKhPublisher.Business.Services
{
    public class AggregatedReportMessageBuilder : IMessageBuilder<AggregatedReportDto>
    {
        public Message Build(AggregatedReportDto report)
        {
            string message = $"{Emoji.HighVoltage} <b>Харківська область станом на {report.Date:dd.MM.yyyy}</b>\n\n";
            message += $"Виявлено:  {report.Confirmed} ({FormatDelta(report.DeltaConfirmed)})\n";
            message += $"Померло: {report.Deaths} ({FormatDelta(report.DeltaDeaths)})\n";
            message += $"Одужало: {report.Recovered} ({FormatDelta(report.DeltaRecovered)})\n";
            message += $"Хворіє: {report.Existing} ({FormatDelta(report.DeltaExisting)})\n\n";

            message += $"{Emoji.Chart} <b>Середній приріст нових випадків по тижням</b>\n\n";
            message += $"Поточний тиждень{(string.IsNullOrEmpty(FormatWeekRange(report.Date)) ? string.Empty : $" ({FormatWeekRange(report.Date)})")}: {FormatDelta(report.AverageGrowthOfConfirmed.CurrentWeek)} в день\n";

            if (report.AverageGrowthOfConfirmed.PreviousWeek > 0)
            {
                message += $"Попередній тиждень: {FormatDelta(report.AverageGrowthOfConfirmed.PreviousWeek)} в день\n";
            }

            if (report.AverageGrowthOfConfirmed.TwoWeeksAgo > 0)
            {
                message += $"Два тижні тому: {FormatDelta(report.AverageGrowthOfConfirmed.TwoWeeksAgo)} в день\n";
            }

            if (report.AverageGrowthOfConfirmed.MonthAgo > 0)
            {
                message += $"Місяць тому: {FormatDelta(report.AverageGrowthOfConfirmed.MonthAgo)} в день\n";
            }

            if (report.MaxGrowthOfConfirmed != null)
            {
                message += $"\n{Emoji.HandPointingRight} Найбільший приріст нових випадків за добу був зафіксований {report.MaxGrowthOfConfirmed.Date:dd.MM.yyyy} і складає {FormatDelta(report.MaxGrowthOfConfirmed.Value)}\n";
            }

            message += $"\n{Emoji.Satellite} Дані надано <a href=\"https://covid19.rnbo.gov.ua/\">Апаратом РНБО України</a>";

            return new Message(message);
        }

        private string FormatDelta(double delta)
        {
            var stringRepresentation = delta.ToString();

            if (delta >= 0)
            {
                stringRepresentation = $"+{stringRepresentation}";
            }

            return stringRepresentation;
        }

        private string FormatWeekRange(DateTime currentDate)
        {
            if (currentDate.Date.DayOfWeek == currentDate.BeginningOfWeek().DayOfWeek)
            {
                return $"{currentDate:ddd}";
            }

            if (currentDate.Date.DayOfWeek == currentDate.EndOfWeek().DayOfWeek)
            {
                return string.Empty;
            }

            return $"{currentDate.BeginningOfWeek():ddd}-{currentDate.Date:ddd}";
        }
    }
}
