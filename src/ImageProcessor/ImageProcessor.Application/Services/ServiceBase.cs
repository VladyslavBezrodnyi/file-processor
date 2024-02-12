using AutoMapper;
using ImageProcessor.Domain.Entities;
using ImageProcessor.Infrastructure.Data.Interfaces;
using ImageProcessor.Infrastructure.Messaging.Interfaces;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Extensions.DependencyInjection;

namespace ImageProcessor.Application.Services
{
    public abstract class ServiceBase(IServiceProvider serviceProvide)
    {
        #region Private Fields

        private readonly IServiceProvider _serviceProvider = serviceProvide;

        private IMapper? _mapper;

        private IRepository<FileMetadata, Guid>? _fileMetadataRepository;
        private IRepository<ProcessEvent, Guid>? _processEventRepository;
        private IBlobStorageClient? _blobStorageClient;

        private IMessageProducer? _messageProducer;

        private ComputerVisionClient? _computerVisionClient;

        #endregion

        #region Public Property

        protected IMapper Mapper => 
            _mapper ??= (_serviceProvider.GetService<IMapper>() ?? throw new NotImplementedException());

        protected IRepository<FileMetadata, Guid> FileMetadataRepository => 
            _fileMetadataRepository ??= (_serviceProvider.GetService<IRepository<FileMetadata, Guid>>() ?? throw new NotImplementedException());

        protected IRepository<ProcessEvent, Guid> ProcessEventRepository =>
            _processEventRepository ??= (_serviceProvider.GetService<IRepository<ProcessEvent, Guid>>() ?? throw new NotImplementedException());

        protected IBlobStorageClient BlobStorageClient =>
            _blobStorageClient ??= (_serviceProvider.GetService<IBlobStorageClient>() ?? throw new NotImplementedException());

        protected IMessageProducer MessageProducer =>
            _messageProducer ??= (_serviceProvider.GetService<IMessageProducer>() ?? throw new NotImplementedException());

        protected ComputerVisionClient ComputerVisionClient =>
            _computerVisionClient ??= (_serviceProvider.GetService<ComputerVisionClient>() ?? throw new NotImplementedException());

        #endregion
    }
}
