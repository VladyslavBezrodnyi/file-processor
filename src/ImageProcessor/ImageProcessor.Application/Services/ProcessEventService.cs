using ImageProcessor.Application.Dtos;
using ImageProcessor.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<ProcessEventDto>> GetByFileIdAsync(Guid fileId)
        {
            var processEvents = await ProcessEventRepository
                .GetByFileId(fileId)
                .ToListAsync();

            return Mapper.Map<IEnumerable<ProcessEventDto>>(processEvents);
        }
    }
}
