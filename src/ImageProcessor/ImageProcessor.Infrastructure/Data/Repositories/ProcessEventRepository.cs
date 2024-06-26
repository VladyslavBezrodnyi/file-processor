﻿using ImageProcessor.Domain.Entities;
using ImageProcessor.Infrastructure.Data.Context;
using ImageProcessor.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImageProcessor.Infrastructure.Data.Repositories
{
    public class ProcessEventRepository(PostgresqlDbContext context) : IProcessEventRepository
    {
        private readonly PostgresqlDbContext _context = context ?? throw new ArgumentNullException();

        public async Task<ProcessEvent?> GetById(Guid id)
        {
            return await _context.ProcessEvents
                .Include(p => p.FileMetadata)
                .FirstOrDefaultAsync(e => e.EventId == id);
        }

        public IQueryable<ProcessEvent> GetByFileId(Guid fileId)
        {
            return _context.ProcessEvents
                .Include(p => p.FileMetadata)
                .Where(e => e.FileId == fileId);
        }

        public async Task<ProcessEvent> CreateAsync(ProcessEvent entity)
        {
            var result = await _context.ProcessEvents.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<ProcessEvent> UpdateAsync(ProcessEvent entity)
        {
            var result = _context.ProcessEvents.Update(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
