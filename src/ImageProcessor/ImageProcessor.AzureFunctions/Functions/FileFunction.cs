using ImageProcessor.Application.Dtos;
using ImageProcessor.Application.Services.Interfaces;
using ImageProcessor.Domain.Extensions;
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
            var file = req.Form.Files["File"];
            Stream stream = file.OpenReadStream();

            var fileType = file.FileName.GetFileTypeFromFileName();
            if (fileType is null)
            {
                return new BadRequestObjectResult("Invalid file extension!");
            }

            var metadataDto = new FileMetadataDto()
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileType = fileType.Value
            };

            var createdMetadataDto = await _fileService.UploadFileAsync(metadataDto, stream);

            if (createdMetadataDto is null)
            {
                return new BadRequestObjectResult("File was not uploaded!");
            }
            return new OkObjectResult(createdMetadataDto);
        }
    }
}
