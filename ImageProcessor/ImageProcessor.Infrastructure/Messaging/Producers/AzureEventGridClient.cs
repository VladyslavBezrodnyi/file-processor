using ImageProcessor.Infrastructure.ConfigurationOptions;
using ImageProcessor.Infrastructure.Messaging.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;

namespace ImageProcessor.Infrastructure.Messaging.Producers
{
    public class AzureEventGridClient : IMessageProducer
    {
        public readonly string _eventGridKey;
        public readonly HttpClient _httpClient;

        public AzureEventGridClient(IOptions<QueueOptions> options)
        {
            _eventGridKey = options.Value.EventGridTopicEndpointUri;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("aeg-sas-key", options.Value.EventGridKey);
        }

        public async Task SendMessageAsync<TData>(IQueueMessage<TData> message)
        {
            var content = new StringContent(message.Serialize(), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_eventGridKey, content);
        }
    }
}
