using ImageProcessor.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.AzureFunctions.Functions
{
    public class ProcessEventFunction(
        IProcessEventService processEventService,
        ILogger<ProcessEventFunction> logger)
    {
        private readonly IProcessEventService _processEventRepository = processEventService;
        private readonly ILogger<ProcessEventFunction> _logger = logger;

        [Function("process-event")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            var eventId = Guid.Parse(req.Query["EventId"].ToString());
            var processEventDto = await _processEventRepository.GetProcessEventAsync(eventId);
            return new OkObjectResult(processEventDto);
        }
    }
}
