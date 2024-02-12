using Azure.Messaging.ServiceBus;
using ImageProcessor.Infrastructure.Messaging.Interfaces;

namespace ImageProcessor.Infrastructure.Messaging.Producers
{
    public class AzureMessageBusProducer(ServiceBusClient client) : IMessageProducer
    {
        private readonly ServiceBusClient client = client;

        public MessageBusSender SetQueueName(QueueNames name)
        {
            return new MessageBusSender(client.CreateSender(name.Value));
        }

        public class MessageBusSender(ServiceBusSender sender) : IMessageSender
        {
            private readonly ServiceBusSender _sender = sender;

            public async Task SendMessageAsync<TData>(IQueueMessage<TData> message)
            {
                var queueMessage = new ServiceBusMessage(message.Serialize());
                await _sender.SendMessageAsync(queueMessage);
            }
        }
    }
}
