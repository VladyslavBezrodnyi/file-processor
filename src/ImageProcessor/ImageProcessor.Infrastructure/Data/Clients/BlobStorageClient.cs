using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Enums;
using ImageProcessor.Infrastructure.ConfigurationOptions;
using ImageProcessor.Infrastructure.Data.Interfaces;
using Microsoft.Extensions.Options;

namespace ImageProcessor.Infrastructure.Data.Clients
{
    public class BlobStorageClient(IOptions<BlobStorageOptions> options) : IBlobStorageClient
    {
        public readonly BlobContainerClient _blobContainerClient = new(options.Value.AzureWebJobsStorage, options.Value.BlobContainerName);

        public async Task<Response<BlobDownloadResult>?> ReadFileAsync(Guid fileId, FileType fileType)
        {
            var blobName = string.Format($"{fileId}.{fileType.ToString().ToLower()}");
            var container = _blobContainerClient.GetBlobClient(blobName);
            if (container is null)
            {
                return null;
            }
            return await container.DownloadContentAsync();
        }

        public Pageable<TaggedBlobItem> ReadFileAsync(string fileName)
        {
            return _blobContainerClient.FindBlobsByTags($"FileName='{fileName}'");
        }

        public async Task<Response<BlobContentInfo>> UploadFileAsync(FileMetadata metadata, Stream file)
        {
            var options = new BlobUploadOptions
            {
                Tags = new Dictionary<string, string>
                {
                    ["FileId"] = metadata.FileId.ToString(),
                    ["FileName"] = metadata.FileName
                },
                Metadata = new Dictionary<string, string>
                {
                    ["FileId"] = metadata.FileId.ToString(),
                    ["FileName"] = metadata.FileName
                },
                Conditions = null
            };

            var blobName = string.Format($"{metadata.FileId}.{metadata.FileType.ToString().ToLower()}");
            return await _blobContainerClient
                .GetBlobClient(blobName)
                .UploadAsync(
                    content: file,
                    options: options);
        }
    }
}
