using Azure.Storage.Queues.Specialized;

namespace ImageProcessor.Infrastructure.Messaging
{
    public class QueueNames
    {
        public readonly static QueueNames FileProcessingQueueName;
        public readonly static QueueNames ClientEventQueueName;

        static QueueNames()
        {
            FileProcessingQueueName = new("file-processing");
            ClientEventQueueName = new("signalr-messaging");
        }

        private QueueNames(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}
