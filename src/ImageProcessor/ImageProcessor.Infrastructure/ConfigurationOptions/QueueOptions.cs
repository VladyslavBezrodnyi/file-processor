namespace ImageProcessor.Infrastructure.ConfigurationOptions
{
    public class QueueOptions
    {
        #region EventGrid

        public string EventGridTopicEndpointUri { get; set; } = String.Empty;
        public string EventGridKey { get; set; } = String.Empty;

        #endregion

        #region ServiceBus

        public string ServiceBusNamespace { get; set; } = String.Empty;
        public string ServiceBusQueueName { get; set; } = String.Empty;

        #endregion
    }
}
