using ImageProcessor.Application.Dtos;
using ImageProcessor.Application.Services.Interfaces;

namespace ImageProcessor.Application.Services
{
    public class ProcessEventService(IServiceProvider serviceProvider)
        : ServiceBase(serviceProvider), IProcessEventService
    {
        public async Task<ProcessEventDto> GetProcessEventAsync(Guid eventId)
        {
            var processEvent = await ProcessEventRepository.GetById(eventId);
            return Mapper.Map<ProcessEventDto>(processEvent);
        }
    }
}
