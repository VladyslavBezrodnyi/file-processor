using Azure;
using Azure.Storage.Blobs.Models;
using ImageProcessor.Domain.Entities;

namespace ImageProcessor.Infrastructure.Data.Interfaces
{
    public interface IBlobStorageClient
    {
        Task<Response<BlobDownloadResult>?> ReadFileAsync(Guid fileId);

        Task<Azure.Response<BlobContentInfo>> UploadFileAsync(FileMetadata metadata, Stream file);
    }
}
