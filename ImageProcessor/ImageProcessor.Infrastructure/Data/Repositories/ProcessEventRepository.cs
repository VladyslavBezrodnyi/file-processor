﻿using ImageProcessor.Domain.Entities;
using ImageProcessor.Domain.Interfaces.Repositories;
using ImageProcessor.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ImageProcessor.Infrastructure.Data.Repositories
{
    public class ProcessEventRepository(PostgresqlDbContext context) : IRepository<ProcessEvent, Guid>
    {
        private readonly PostgresqlDbContext _context = context;

        public async Task<ProcessEvent> GetById(Guid id)
        {
            return await _context.ProcessEvents.FirstOrDefaultAsync(e => e.EventId == id);
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
