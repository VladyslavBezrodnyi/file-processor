using ImageProcessor.Application.Dtos;
using ImageProcessor.Application.Services.Interfaces;
using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Entities.Result;
using ImageProcessor.Domain.Enums;
using ImageProcessor.Infrastructure.Messaging;
using ImageProcessor.Infrastructure.Messaging.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace ImageProcessor.Application.Services
{
    public class VisionService(IServiceProvider serviceProvider) 
        : ServiceBase(serviceProvider), IVisionService
    {
        private static readonly QueueNames ClientEvent = QueueNames.ClientEventQueueName;

        public async Task<ProcessEventDto> ProcessImageAsync(Guid eventId)
        {
            var processEvent = await ProcessEventRepository.GetById(eventId);
            var metadata = processEvent.FileMetadata;
            processEvent = await UpdateStatus(processEvent, ProcessStatus.InProcess);

            try
            {
                var img = await BlobStorageClient.ReadFileAsync(processEvent.FileId, metadata.FileType);

                if (img?.Value?.Content is null)
                {
                    processEvent = await UpdateStatus(processEvent, ProcessStatus.Faild);
                    return Mapper.Map<ProcessEventDto>(processEvent);
                }

                var result = await RecognizeTextAsync(img.Value.Content.ToStream());
                processEvent.Output = result;
                processEvent = await UpdateStatus(processEvent, ProcessStatus.Success);
                return Mapper.Map<ProcessEventDto>(processEvent);
            }
            catch (Exception ex)
            {
                processEvent.FaildMessage = ex.Message;
                processEvent = await UpdateStatus(processEvent, ProcessStatus.Faild);
                return Mapper.Map<ProcessEventDto>(processEvent);
            }
        }

        private async Task<OCRResult> RecognizeTextAsync(Stream image)
        {
            var textHeaders = await ComputerVisionClient.ReadInStreamAsync(image);
            string operationLocation = textHeaders.OperationLocation;
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);
            ReadOperationResult results;
            do
            {
                results = await ComputerVisionClient.GetReadResultAsync(Guid.Parse(operationId));
            }
            while (results.Status is OperationStatusCodes.Running or OperationStatusCodes.NotStarted);

            var lines = new List<string>();
            foreach (ReadResult page in results.AnalyzeResult.ReadResults)
            {
                foreach (Line line in page.Lines)
                {
                    lines.Add(line.Text);
                }
            }
            return new OCRResult()
            {
                Lines = lines
            };
        }

        private async Task<ProcessEvent> UpdateStatus(ProcessEvent processEvent, ProcessStatus status)
        {
            processEvent.ProcessStatus = status;
            var updatedEvent = await ProcessEventRepository.UpdateAsync(processEvent);

            var message = new AzureServiceBusMessage<ProcessEventDto>()
            {
                Id = Guid.NewGuid().ToString(),
                Topic = "process-event",
                Subject = "process-event",
                EventType = "trigger",
                Payload = Mapper.Map<ProcessEventDto>(updatedEvent)
            };
            await MessageProducer.SetQueueName(ClientEvent)
                .SendMessageAsync(message);
            return updatedEvent;
        }
    }
}
