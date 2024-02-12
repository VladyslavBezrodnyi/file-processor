using Azure.Messaging.ServiceBus;
using ImageProcessor.Application.Dtos;
using ImageProcessor.Application.Services.Interfaces;
using ImageProcessor.Infrastructure.Messaging.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.AzureFunctions.BackgroundHandlers
{
    public class FileProcessingHandler(
        IVisionService fileProcessorService,
        ILogger<FileProcessingHandler> logger)
    {
        private readonly IVisionService _fileProcessorService = fileProcessorService;
        private readonly ILogger<FileProcessingHandler> _logger = logger;

        [Function("image-processing-handler")]
        public async Task Run(
            [ServiceBusTrigger("file-processing", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {messageId}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            var messageString = message.Body.ToString();
            var entity = AzureServiceBusMessage<ProcessEventDto>.DeserializeAzureEventGridEvent(messageString);

            if (entity?.Payload is null)
            {
                _logger.LogError("Invalid payload!");
                await messageActions.CompleteMessageAsync(message);
                return;
            }

            await _fileProcessorService.ProcessImageAsync(entity.Payload.EventId);

            await messageActions.CompleteMessageAsync(message);
        }
    }
}
