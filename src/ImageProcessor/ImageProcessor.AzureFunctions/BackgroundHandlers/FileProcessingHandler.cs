using Azure.Messaging.ServiceBus;
using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Interfaces.Services;
using ImageProcessor.Infrastructure.Messaging.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.AzureFunctions.BackgroundHandlers
{
    public class FileProcessingHandler
    {
        private readonly IVisionService _fileProcessorService;
        private readonly ILogger<FileProcessingHandler> _logger;

        public FileProcessingHandler(
            IVisionService fileProcessorService,
            ILogger<FileProcessingHandler> logger)
        {
            _fileProcessorService = fileProcessorService;
            _logger = logger;
        }

        [Function("image-processing-handler")]
        public async Task Run(
            [ServiceBusTrigger("file-processing", Connection = "ServiceBusConnection")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {messageId}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            var messageString = message.Body.ToString();
            var entity = AzureServiceBusMessage<ProcessEvent>.DeserializeAzureEventGridEvent(messageString);

            if (entity?.Payload is null)
            {
                _logger.LogError("Invalid payload!");
                await messageActions.CompleteMessageAsync(message);
            }

            await _fileProcessorService.ProcessImageAsync(entity.Payload);

            await messageActions.CompleteMessageAsync(message);
        }
    }
}
