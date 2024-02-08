using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.AzureFunctions.Functions
{
    public class ProcessEventFunction(
        IRepository<ProcessEvent, Guid> processEventRepository,
        ILogger<ProcessEventFunction> logger)
    {
        private readonly IRepository<ProcessEvent, Guid> _processEventRepository = processEventRepository;
        private readonly ILogger<ProcessEventFunction> _logger = logger;

        [Function("process-event")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var eventId = Guid.Parse(req.Headers["EventId"].ToString());
            var result = await _processEventRepository.GetById(eventId);
            result.FileMetadata.ProcessEvents = null;
            return new OkObjectResult(result);
        }
    }
}
