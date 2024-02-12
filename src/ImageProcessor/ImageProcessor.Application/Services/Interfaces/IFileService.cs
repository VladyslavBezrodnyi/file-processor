using ImageProcessor.Application.Dtos;
using ImageProcessor.Domain.Enums;

namespace ImageProcessor.Application.Services.Interfaces
{
    public interface IFileService
    {
        Task<FileMetadataDto?> UploadFileAsync(FileMetadataDto metadataToCreate, Stream file);
        Task<ProcessEventDto?> TriggerProcessingAsync(Guid fileId, ProcessType processType);
    }
}
