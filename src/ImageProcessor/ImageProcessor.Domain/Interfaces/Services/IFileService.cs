using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Enums;

namespace ImageProcessor.Domain.Interfaces.Services
{
    public interface IFileService
    {
        Task<FileMetadata?> UploadFileAsync(FileMetadata metadataToCreate, Stream file);
        Task<ProcessEvent?> TriggerProcessingAsync(Guid fileId, ProcessType processType);
    }
}
