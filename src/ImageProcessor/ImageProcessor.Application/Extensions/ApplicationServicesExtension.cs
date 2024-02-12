using AutoMapper;
using ImageProcessor.Application.Options;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ImageProcessor.Application.Extensions
{
    public static class ApplicationServicesExtension
    {
        private static readonly Assembly _assembly = typeof(ApplicationServicesExtension).Assembly;

        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            return services.AddAutoMapper(_assembly);
        }

        public static IServiceCollection AddCVClient(this IServiceCollection services)
        {
            return services.AddScoped<ComputerVisionClient>(p =>
            {
                var options = p.GetService<IOptions<CVOptions>>();
                var apiKey = new ApiKeyServiceClientCredentials(options.Value.VisionKey);
                return new ComputerVisionClient(apiKey)
                {
                    Endpoint = options.Value.VisionEndpoint
                };
            });
        }
    }
}
