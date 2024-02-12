namespace ImageProcessor.Infrastructure.ConfigurationOptions
{
    public class QueueOptions
    {

        #region ServiceBus

        public required string ServiceBusNamespace { get; set; }
        public required string ServiceBusConnection { get; set; }

        #endregion
    }
}
