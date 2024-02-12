namespace ImageProcessor.Infrastructure.Messaging.Interfaces
{
    public interface IMessageSender
    {
        Task SendMessageAsync<TData>(IQueueMessage<TData> message);
    }
}
