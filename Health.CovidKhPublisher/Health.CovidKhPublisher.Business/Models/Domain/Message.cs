namespace Health.CovidKhPublisher.Business.Models.Domain
{
    public class Message
    {
        public Message(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
