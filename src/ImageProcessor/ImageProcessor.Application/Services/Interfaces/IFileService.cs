using ImageProcessor.Application.Dtos;
using ImageProcessor.Domain.Enums;

namespace ImageProcessor.Application.Services.Interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<FileMetadataDto>> GetFilesAsync();
        Task<FileMetadataDto?> UploadFileAsync(FileMetadataDto metadataToCreate, Stream file);
        Task<(BlobDetailsDto, BinaryData?)> DownloadFileAsync(Guid fileId);
        Task<ProcessEventDto?> TriggerProcessingAsync(Guid fileId, ProcessType processType);
    }
}
