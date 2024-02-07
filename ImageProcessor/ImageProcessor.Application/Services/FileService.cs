using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Enums;
using ImageProcessor.Domain.Interfaces.Repositories;
using ImageProcessor.Domain.Interfaces.Services;
using ImageProcessor.Infrastructure.Data.Interfaces;
using ImageProcessor.Infrastructure.Messaging.Interfaces;
using ImageProcessor.Infrastructure.Messaging.Models;

namespace ImageProcessor.Application.Services
{
    public class FileService(
        IRepository<FileMetadata, Guid> fileMetadataRepository,
        IRepository<ProcessEvent, Guid> processEventRepository,
        IBlobStorageClient blobStorageClient,
        IMessageProducer messageProducer) : IFileService
    {
        private readonly IRepository<FileMetadata, Guid> _fileMetadataRepository = fileMetadataRepository;
        private readonly IRepository<ProcessEvent, Guid> _processEventRepository = processEventRepository;
        private readonly IBlobStorageClient _blobStorageClient = blobStorageClient;
        private readonly IMessageProducer _messageProducer = messageProducer;

        public async Task<FileMetadata?> UploadFileAsync(FileMetadata metadataToCreate, Stream file)
        {
            var createdMetadata = await _fileMetadataRepository.CreateAsync(metadataToCreate);
            if (createdMetadata is null)
            {
                return null;
            }
            await _blobStorageClient.UploadFileAsync(createdMetadata, file);
            return createdMetadata;
        }

        public async Task<ProcessEvent?> TriggerProcessingAsync(Guid fileId, ProcessType processType)
        {
            var processEventToCreate = new ProcessEvent()
            {
                FileId = fileId,
                ProcessType = processType,
                ProcessStatus = ProcessStatus.InQueue
            };

            var createdEvent = await _processEventRepository.CreateAsync(processEventToCreate);

            if (createdEvent is null)
            {
                return null;
            }

            var message = new AzureServiceBusMessage<ProcessEvent>()
            {
                Id = Guid.NewGuid().ToString(),
                Topic = "process-event",
                Subject = "process-event",
                EventType = "trigger",
                Payload = createdEvent
            };

            await _messageProducer.SendMessageAsync(message);

            return createdEvent;
        }

    }
}
