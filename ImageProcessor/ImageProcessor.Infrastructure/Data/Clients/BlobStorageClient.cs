using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageProcessor.Domain.Entities;
using ImageProcessor.Infrastructure.ConfigurationOptions;
using ImageProcessor.Infrastructure.Data.Interfaces;
using Microsoft.Extensions.Options;

namespace ImageProcessor.Infrastructure.Data.Clients
{
    public class BlobStorageClient : IBlobStorageClient
    {
        public readonly BlobContainerClient _blobContainerClient;

        public BlobStorageClient(IOptions<BlobStorageOptions> options)
        {
            _blobContainerClient = new BlobContainerClient(options.Value.AzureWebJobsStorage, options.Value.BlobContainerName);
        }

        public AsyncPageable<BlobItem> ReadAllAsync()
        {
            return _blobContainerClient.GetBlobsAsync();
        }

        public async Task<Response<BlobDownloadResult>?> ReadFileAsync(Guid fileId)
        {
            var container = _blobContainerClient.GetBlobClient(fileId.ToString());
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
                    ["FileName"] = metadata.FileName
                },
                Metadata = new Dictionary<string, string>
                {
                    ["FileName"] = metadata.FileName
                },
                Conditions = null
            };
            return await _blobContainerClient
                .GetBlobClient(metadata.FileId.ToString())
                .UploadAsync(
                    content: file,
                    options: options);
        }
    }
}
