using ImageProcessor.Domain.Enums;

namespace ImageProcessor.Domain.Entities
{
    public class FileMetadata : BaseEntity
    {
        public Guid FileId { get; set; }

        public required string FileName { get; set; }

        public FileType FileType { get; set; }

        public ICollection<ProcessEvent> ProcessEvents { get; set; }
    }
}
