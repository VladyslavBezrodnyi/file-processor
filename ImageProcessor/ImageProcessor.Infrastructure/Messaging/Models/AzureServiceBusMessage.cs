using ImageProcessor.Infrastructure.Messaging.Interfaces;
using System.Text.Json;

namespace ImageProcessor.Infrastructure.Messaging.Models
{
    public class AzureServiceBusMessage<TData> : IQueueMessage<TData>
    {
        public required string Id { get; set; }

        public required string Topic { get; set; }

        public required string Subject { get; set; }

        public required string EventType { get; set; }

        public DateTime EventTime { get; set; } = DateTime.UtcNow;

        public required TData Payload { get; set; }

        public string Serialize()
        {
            return JsonSerializer.Serialize(this, jsonSerializerSettings);
        }

        public static AzureServiceBusMessage<TData>? DeserializeAzureEventGridEvent(string payload)
        {
            return JsonSerializer.Deserialize<AzureServiceBusMessage<TData>>(payload, jsonSerializerSettings);
        }

        private static JsonSerializerOptions jsonSerializerSettings = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
