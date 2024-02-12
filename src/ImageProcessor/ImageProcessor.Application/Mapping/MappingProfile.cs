using AutoMapper;
using ImageProcessor.Application.Dtos;
using ImageProcessor.Domain.Entities;

namespace ImageProcessor.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FileMetadata, FileMetadataDto>()
                .ReverseMap();
            CreateMap<ProcessEvent, ProcessEventDto>()
                .ReverseMap();
        }
    }
}
