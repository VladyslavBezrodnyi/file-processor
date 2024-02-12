using ImageProcessor.Domain.Enums;
using System.Text.Json.Serialization;

namespace ImageProcessor.Application.Dtos
{
    public class FileMetadataDto
    {
        public Guid FileId { get; set; }

        public required string FileName { get; set; }

        public required string ContentType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FileType FileType { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
