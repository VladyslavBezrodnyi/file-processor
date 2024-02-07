namespace ImageProcessor.Domain.Enums
{
    public enum ProcessStatus
    {
        Unknown = 0,
        ReadyToProcess,
        InQueue,
        InProcess,
        Success,
        Faild
    }
}
