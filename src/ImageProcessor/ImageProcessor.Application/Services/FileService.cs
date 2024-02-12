using ImageProcessor.Application.Dtos;
using ImageProcessor.Application.Services.Interfaces;
using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Enums;
using ImageProcessor.Infrastructure.Messaging;
using ImageProcessor.Infrastructure.Messaging.Models;

namespace ImageProcessor.Application.Services
{
    public class FileService(IServiceProvider serviceProvider) 
        : ServiceBase(serviceProvider), IFileService
    {
        public async Task<FileMetadataDto?> UploadFileAsync(FileMetadataDto metadataToCreateDto, Stream file)
        {
            var metadataToCreate = Mapper.Map<FileMetadata>(metadataToCreateDto);
            var createdMetadata = await FileMetadataRepository.CreateAsync(metadataToCreate);
            if (createdMetadata is null)
            {
                return null;
            }
            await BlobStorageClient.UploadFileAsync(createdMetadata, file);
            return Mapper.Map<FileMetadataDto>(createdMetadata);
        }

        public async Task<ProcessEventDto?> TriggerProcessingAsync(Guid fileId, ProcessType processType)
        {
            var processEventToCreate = new ProcessEvent()
            {
                FileId = fileId,
                ProcessType = processType,
                ProcessStatus = ProcessStatus.InQueue
            };

            var createdEvent = await ProcessEventRepository.CreateAsync(processEventToCreate);

            if (createdEvent is null)
            {
                return null;
            }

            var createdEventDto = Mapper.Map<ProcessEventDto>(createdEvent);

            var message = new AzureServiceBusMessage<ProcessEventDto>()
            {
                Id = Guid.NewGuid().ToString(),
                Topic = "process-event",
                Subject = "process-event",
                EventType = "trigger",
                Payload = createdEventDto
            };

            await MessageProducer
                .SetQueueName(QueueNames.FileProcessingQueueName)
                .SendMessageAsync(message);

            return createdEventDto;
        }

    }
}
