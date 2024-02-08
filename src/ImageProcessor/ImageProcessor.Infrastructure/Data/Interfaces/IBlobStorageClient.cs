using Azure;
using Azure.Storage.Blobs.Models;
using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Enums;

namespace ImageProcessor.Infrastructure.Data.Interfaces
{
    public interface IBlobStorageClient
    {
        Task<Response<BlobDownloadResult>?> ReadFileAsync(Guid fileId, FileType fileType);

        Task<Azure.Response<BlobContentInfo>> UploadFileAsync(FileMetadata metadata, Stream file);
    }
}
