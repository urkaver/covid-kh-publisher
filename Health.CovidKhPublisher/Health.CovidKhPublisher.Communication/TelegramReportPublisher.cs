using System.Threading.Tasks;
using Health.CovidKhPublisher.Business.Interfaces;
using Health.CovidKhPublisher.Configuration;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Message = Health.CovidKhPublisher.Business.Models.Domain.Message;

namespace Health.CovidKhPublisher.Communication
{
    public class TelegramMessagePublisher : IMessagePublisher
    {
        private readonly ITelegramBotClient _botClient;
        private readonly string _channelId;

        public TelegramMessagePublisher(IOptionsSnapshot<AppSettings> appSettings)
        {
            _botClient = new TelegramBotClient(appSettings.Value.TelegramBotAccessToken);
            _channelId = appSettings.Value.TelegramPublishChannel;
        }

        public Task PublishAsync(Message message)
        {
            return _botClient.SendTextMessageAsync(new ChatId(_channelId), message.Text, ParseMode.Html);
        }
    }
}
