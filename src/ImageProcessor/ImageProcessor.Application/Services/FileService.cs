using ImageProcessor.Application.Dtos;
using ImageProcessor.Application.Services.Interfaces;
using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Enums;
using ImageProcessor.Infrastructure.Messaging;
using ImageProcessor.Infrastructure.Messaging.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageProcessor.Application.Services
{
    public class FileService(IServiceProvider serviceProvider) 
        : ServiceBase(serviceProvider), IFileService
    {
        public async Task<IEnumerable<FileMetadataDto>> GetFilesAsync()
        {
            var filesMetada = await FileMetadataRepository
                .GetFiles()
                .ToListAsync();

            return Mapper.Map<IEnumerable<FileMetadataDto>>(filesMetada);
        }

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

        public async Task<(BlobDetailsDto, BinaryData?)> DownloadFileAsync(Guid fileId)
        {
            var metadata = await FileMetadataRepository.GetById(fileId);
            if (metadata is null)
            {
                return (null, null);
            }
            var result = await BlobStorageClient.ReadFileAsync(metadata.FileId, metadata.FileType);
            var details = new BlobDetailsDto()
            {
                BlobContentType = result.Value.Details.ContentType,
                InputContentType = metadata.ContentType
            };
            var content = result?.Value?.Content;
            return (details, content);
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
