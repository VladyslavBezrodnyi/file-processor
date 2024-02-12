using ImageProcessor.Domain.Entities;

namespace ImageProcessor.Infrastructure.Data.Interfaces
{
    public interface IProcessEventRepository : IRepository<ProcessEvent, Guid>
    {
        IQueryable<ProcessEvent> GetByFileId(Guid fileId);
    }
}
