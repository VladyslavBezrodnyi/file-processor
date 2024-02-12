using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.AzureFunctions.Functions
{
    public class FilesFunction(ILogger<FilesFunction> logger)
    {
        private readonly ILogger<FilesFunction> _logger = logger;

        [Function("files")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            return new OkObjectResult("Value");
        }
    }
}
