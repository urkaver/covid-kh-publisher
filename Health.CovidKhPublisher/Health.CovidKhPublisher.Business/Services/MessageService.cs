using System;
using System.Threading.Tasks;
using Health.CovidKhPublisher.Business.Interfaces;
using Health.CovidKhPublisher.Business.Models.Domain;
using Health.CovidKhPublisher.Business.Models.Domain.Reports;
using Health.CovidKhPublisher.Business.Models.Dto;

namespace Health.CovidKhPublisher.Business.Services
{
    public class MessageService : IMessageService
    {
        private readonly IReportService _reportService;
        private readonly IMessageBuilder<AggregatedReportDto> _messageBuilder;
        private readonly IMessagePublisher _messagePublisher;

        public MessageService(IReportService reportService, IMessageBuilder<AggregatedReportDto> messageBuilder, IMessagePublisher messagePublisher)
        {
            _reportService = reportService;
            _messageBuilder = messageBuilder;
            _messagePublisher = messagePublisher;
        }

        public async Task SendDayReportMessageAsync(Location location, DateTime datetime)
        {
            AggregatedReportDto dayReport = await _reportService.GetByDateAsync(location, datetime);

            Message message = _messageBuilder.Build(dayReport);

            await _messagePublisher.PublishAsync(message);
        }
    }
}
