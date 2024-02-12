using ImageProcessor.Infrastructure.Messaging.Producers;

namespace ImageProcessor.Infrastructure.Messaging.Interfaces
{
    public interface IMessageProducer
    {
        AzureMessageBusProducer.MessageBusSender SetQueueName(QueueNames name);
    }
}
