using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace ImageProcessor.AzureFunctions.SignalR
{
    public class NegotiateFunction
    {
        
        [Function("negotiate")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            [SignalRConnectionInfoInput(HubName = "messaging", ConnectionStringSetting = "AzureSignalRConnectionString")] string connectionInfo)
        {
            return connectionInfo != null
                ? (ActionResult)new OkObjectResult(connectionInfo)
                : new NotFoundObjectResult("Failed to load SignalR Info.");
        }
    }
}
