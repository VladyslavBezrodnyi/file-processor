using ImageProcessor.Application.Dtos;

namespace ImageProcessor.Application.Services.Interfaces
{
    public interface IProcessEventService
    {
        Task<ProcessEventDto> GetProcessEventAsync(Guid eventId);
    }
}
