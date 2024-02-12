using Azure.Messaging.ServiceBus;
using ImageProcessor.Application.Dtos;
using ImageProcessor.Infrastructure.Messaging.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.AzureFunctions.SignalR
{
    public class BroadcastFunction(ILogger<BroadcastFunction> logger)
    {
        private readonly ILogger<BroadcastFunction> _logger = logger;

        [Function("broadcast")]
        [SignalROutput(HubName = "chat", ConnectionStringSetting = "AzureSignalRConnectionString")]
        public SignalRMessageAction Broadcast(
            [ServiceBusTrigger("signalr-messaging", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
        {
            _logger.LogInformation("Message ID: {messageId}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            var messageString = message.Body.ToString();
            var entity = AzureServiceBusMessage<ProcessEventDto>.DeserializeAzureEventGridEvent(messageString);

            if (entity?.Payload is null)
            {
                _logger.LogError("Invalid payload!");
                throw new Exception();
            }

            return new SignalRMessageAction("status")
            {
                // broadcast to all the connected clients without specifying any connection, user or group.
                Arguments = new[] { entity.Payload }
            };
        }
    }
}
