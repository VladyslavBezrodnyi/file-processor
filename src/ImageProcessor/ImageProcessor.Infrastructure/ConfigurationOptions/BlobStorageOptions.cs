namespace ImageProcessor.Infrastructure.ConfigurationOptions
{
    public class BlobStorageOptions
    {
        public string AzureWebJobsStorage { get; set; } = String.Empty;
        public string BlobContainerName { get; set; } = String.Empty;
    }
}
