using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Enums;
using ImageProcessor.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.AzureFunctions.Functions
{
    public class FileFunction(
        IFileService fileService,
        ILogger<FileFunction> logger)
    {
        private readonly IFileService _fileService = fileService;
        private readonly ILogger<FileFunction> _logger = logger;

        [Function("file")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            switch (req.Method.ToLower())
            {
                case "get":
                    return await GetRequest(req);
                case "post":
                    return await PostRequest(req);
                default:
                    throw new NotImplementedException();
            }
        }

        private async Task<IActionResult> GetRequest(HttpRequest req)
        {
            //TODO
            return new OkObjectResult("Value");
        }

        private async Task<IActionResult> PostRequest(HttpRequest req)
        {
            Stream stream = new MemoryStream();
            var file = req.Form.Files["File"];
            stream = file.OpenReadStream();

            var metadata = new FileMetadata()
            {
                FileName = file.FileName,
                FileType = FileType.PNG
            };

            var createdMetadata = await _fileService.UploadFileAsync(metadata, stream);

            if (createdMetadata is null)
            {
                return new BadRequestObjectResult("File was not uploaded!");
            }
            return new OkObjectResult(createdMetadata);
        }
    }
}
