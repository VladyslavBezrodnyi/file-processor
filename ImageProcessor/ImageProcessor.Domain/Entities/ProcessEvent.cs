using ImageProcessor.Domain.Enums;

namespace ImageProcessor.Domain.Entities
{
    public class ProcessEvent : BaseEntity
    {
        public Guid EventId { get; set; }

        public Guid FileId { get; set; }

        public ProcessType ProcessType { get; set; }

        public ProcessStatus ProcessStatus { get; set; }

        public string? Input { get; set; }

        public string? Output { get; set; }

        public string? FaildMessage { get; set; }

        public FileMetadata FileMetadata { get; set; }
    }
}
