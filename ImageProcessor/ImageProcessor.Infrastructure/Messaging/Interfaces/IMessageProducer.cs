namespace ImageProcessor.Infrastructure.Messaging.Interfaces
{
    public interface IMessageProducer
    {
        Task SendMessageAsync<TData>(IQueueMessage<TData> message);
    }
}
