namespace ImageProcessor.Domain.Entities.Result
{
    public class OCRResult
    {
        public ICollection<string> Lines { get; set; } = new List<string>();
    }
}
