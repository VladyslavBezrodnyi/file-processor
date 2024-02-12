using ImageProcessor.Domain.Enums;
using System.Text.Json.Serialization;

namespace ImageProcessor.Application.Dtos
{
    public class ProcessEventDto
    {
        public Guid EventId { get; set; }

        public Guid FileId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProcessType ProcessType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProcessStatus ProcessStatus { get; set; }

        public object? Input { get; set; }

        public object? Output { get; set; }

        public string? FaildMessage { get; set; }

        public DateTime UpdatedDate { get; set; }

        public FileMetadataDto FileMetadata { get; set; }
    }
}
