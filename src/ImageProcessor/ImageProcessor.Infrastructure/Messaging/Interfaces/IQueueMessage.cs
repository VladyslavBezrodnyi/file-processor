namespace ImageProcessor.Infrastructure.Messaging.Interfaces
{
    public interface IQueueMessage<TData>
    {
        string Id { get; set; }

        string Topic { get; set; }

        string Subject { get; set; }

        string EventType { get; set; }

        DateTime EventTime { get; set; }

        TData Payload { get; set; }

        string Serialize();
    }
}
