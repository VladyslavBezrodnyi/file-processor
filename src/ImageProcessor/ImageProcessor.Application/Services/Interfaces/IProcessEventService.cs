using ImageProcessor.Application.Dtos;
using ImageProcessor.Domain.Entities;

namespace ImageProcessor.Application.Services.Interfaces
{
    public interface IProcessEventService
    {
        Task<ProcessEventDto> GetProcessEventAsync(Guid eventId);

        Task<IEnumerable<ProcessEventDto>> GetByFileIdAsync(Guid id);
    }
}
