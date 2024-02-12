namespace ImageProcessor.Infrastructure.ConfigurationOptions
{
    public class BlobStorageOptions
    {
        public required string AzureWebJobsStorage { get; set; }
        public required string BlobContainerName { get; set; }
    }
}
