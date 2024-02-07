using ImageProcessor.Application.Options;
using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Interfaces.Repositories;
using ImageProcessor.Domain.Interfaces.Services;
using ImageProcessor.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using ImageProcessor.Infrastructure.Data.Clients;
using ImageProcessor.Domain.Enums;
using System.IO;
using System.Text;

namespace ImageProcessor.Application.Services
{
    public class VisionService : IVisionService
    {
        private readonly IRepository<ProcessEvent, Guid> _processEventRepository;
        private readonly IBlobStorageClient _blobStorageClient;
        private readonly ComputerVisionClient _computerVisionClient;

        public VisionService(
            IOptions<CVOptions> options,
            IRepository<ProcessEvent, Guid> processEventRepository,
            IBlobStorageClient blobStorageClient)
        {
            _processEventRepository = processEventRepository;
            _blobStorageClient = blobStorageClient;

            var apiKey = new ApiKeyServiceClientCredentials(options.Value.VisionKey);
            _computerVisionClient = new ComputerVisionClient(apiKey)
            { 
                Endpoint = options.Value.VisionEndpoint 
            };
        }

        public async Task<ProcessEvent> ProcessImageAsync(ProcessEvent processEvent)
        {
            processEvent = await _processEventRepository.GetById(processEvent.EventId);
            processEvent.ProcessStatus = ProcessStatus.InProcess;
            processEvent = await _processEventRepository.UpdateAsync(processEvent);

            try
            {
                var img = await _blobStorageClient.ReadFileAsync(processEvent.FileId);

                if (img?.Value?.Content is null)
                {
                    processEvent.ProcessStatus = ProcessStatus.Faild;
                    await _processEventRepository.UpdateAsync(processEvent);
                    return processEvent;
                }

                processEvent.ProcessStatus = ProcessStatus.Success;
                processEvent.Output = await RecognizeTextAsync(img.Value.Content.ToStream());
                processEvent = await _processEventRepository.UpdateAsync(processEvent);
                return processEvent;
            }
            catch (Exception ex)
            {
                processEvent.ProcessStatus = ProcessStatus.Faild;
                processEvent.FaildMessage = ex.Message;
                processEvent = await _processEventRepository.UpdateAsync(processEvent);
                return processEvent;
            }
        }

        private async Task<string> RecognizeTextAsync(Stream image)
        {
            var textHeaders = await _computerVisionClient.ReadInStreamAsync(image);
            string operationLocation = textHeaders.OperationLocation;
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);
            ReadOperationResult results;
            do
            {
                results = await _computerVisionClient.GetReadResultAsync(Guid.Parse(operationId));
            }
            while (results.Status is OperationStatusCodes.Running or OperationStatusCodes.NotStarted);

            StringBuilder resultText = new StringBuilder();
            foreach (ReadResult page in results.AnalyzeResult.ReadResults)
            {
                foreach (Line line in page.Lines)
                {
                    resultText.AppendLine(line.Text);
                }
            }
            return resultText.ToString();
        }
    }
}
