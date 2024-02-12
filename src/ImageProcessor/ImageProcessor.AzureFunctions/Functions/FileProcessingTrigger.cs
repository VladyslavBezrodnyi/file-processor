using ImageProcessor.Application.Services.Interfaces;
using ImageProcessor.Domain.Enums;
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
            var fileId = Guid.Parse(req.Query["FileId"].ToString());

            var processEvent = await _fileService.TriggerProcessingAsync(fileId, ProcessType.OCR);

            return new OkObjectResult(processEvent);
        }
    }
}
