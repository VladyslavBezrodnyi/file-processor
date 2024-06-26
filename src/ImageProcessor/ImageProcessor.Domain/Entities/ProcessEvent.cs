﻿using ImageProcessor.Domain.Enums;

namespace ImageProcessor.Domain.Entities
{
    public class ProcessEvent : BaseEntity
    {
        public Guid EventId { get; set; }

        public Guid FileId { get; set; }

        public ProcessType ProcessType { get; set; }

        public ProcessStatus ProcessStatus { get; set; }

        public object? Input { get; set; }

        public object? Output { get; set; }

        public string? FaildMessage { get; set; }

        public FileMetadata FileMetadata { get; set; }
    }
}
