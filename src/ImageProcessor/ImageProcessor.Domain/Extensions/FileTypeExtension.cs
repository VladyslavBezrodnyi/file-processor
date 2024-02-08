using ImageProcessor.Domain.Enums;

namespace ImageProcessor.Domain.Extensions
{
    public static class FileTypeExtension
    {
        public static FileType? GetFileTypeFromFileName(this string fileName)
        {
            return (Path.GetExtension(fileName)?.ToLower()) switch
            {
                ".png" => (FileType?)FileType.PNG,
                ".jpg" => (FileType?)FileType.JPG,
                ".pdf" => (FileType?)FileType.PDF,
                _ => null,
            };
        }
    }
}
