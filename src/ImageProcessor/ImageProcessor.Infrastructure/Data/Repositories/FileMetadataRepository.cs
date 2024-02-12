using ImageProcessor.Domain.Entities;
using ImageProcessor.Infrastructure.Data.Context;
using ImageProcessor.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImageProcessor.Infrastructure.Data.Repositories
{
    public class FileMetadataRepository(PostgresqlDbContext context) : IRepository<FileMetadata, Guid>
    {
        private readonly PostgresqlDbContext _context = context;

        public async Task<FileMetadata> GetById(Guid id)
        {
            return await _context.FileMetadata.FirstOrDefaultAsync(e => e.FileId == id);
        }

        public async Task<FileMetadata> CreateAsync(FileMetadata entity)
        {
            var result = await _context.FileMetadata.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<FileMetadata> UpdateAsync(FileMetadata entity)
        {
            var result = _context.FileMetadata.Update(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
