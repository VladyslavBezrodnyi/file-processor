using ImageProcessor.Domain.Enums;
using ImageProcessor.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.AzureFunctions.Functions
{
    public class FileProcessingTrigger(
        IFileService fileService,
        ILogger<FileProcessingTrigger> logger)
    {
        private readonly IFileService _fileService = fileService;
        private readonly ILogger<FileProcessingTrigger> _logger = logger;

        [Function("file-processing-trigger")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var fileId = Guid.Parse(req.Headers["FileId"].ToString());

            var processEvent = await _fileService.TriggerProcessingAsync(fileId, ProcessType.OCR);

            return new OkObjectResult(processEvent);
        }
    }
}
