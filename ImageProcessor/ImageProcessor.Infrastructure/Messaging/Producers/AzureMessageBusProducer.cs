using Azure.Identity;
using Azure.Messaging.ServiceBus;
using ImageProcessor.Infrastructure.ConfigurationOptions;
using ImageProcessor.Infrastructure.Messaging.Interfaces;
using Microsoft.Extensions.Options;

namespace ImageProcessor.Infrastructure.Messaging.Producers
{
    public class AzureMessageBusProducer : IMessageProducer
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;

        public AzureMessageBusProducer(IOptions<QueueOptions> options) 
        {
            var clientOptions = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            _client = new ServiceBusClient(
                    options.Value.ServiceBusNamespace,
                    new DefaultAzureCredential(),
                    clientOptions);
            _sender = _client.CreateSender(options.Value.ServiceBusQueueName);
        }

        public async Task SendMessageAsync<TData>(IQueueMessage<TData> message)
        {
            var queueMessage = new ServiceBusMessage(message.Serialize());
            await _sender.SendMessageAsync(queueMessage);
        }
    }
}
