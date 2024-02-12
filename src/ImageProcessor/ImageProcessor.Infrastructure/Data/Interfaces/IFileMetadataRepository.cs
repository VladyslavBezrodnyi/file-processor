using ImageProcessor.Domain.Entities;

namespace ImageProcessor.Infrastructure.Data.Interfaces
{
    public interface IFileMetadataRepository : IRepository<FileMetadata, Guid>
    {
        IQueryable<FileMetadata> GetFiles();
    }
}
