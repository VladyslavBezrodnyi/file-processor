using ImageProcessor.Application.Dtos;

namespace ImageProcessor.Application.Services.Interfaces
{
    public interface IVisionService
    {
        Task<ProcessEventDto> ProcessImageAsync(Guid eventId);
    }
}
