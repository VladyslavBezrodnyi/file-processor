using ImageProcessor.Domain.Entities;

namespace ImageProcessor.Domain.Interfaces.Services
{
    public interface IVisionService
    {
        Task<ProcessEvent> ProcessImageAsync(ProcessEvent processEvent);
    }
}
