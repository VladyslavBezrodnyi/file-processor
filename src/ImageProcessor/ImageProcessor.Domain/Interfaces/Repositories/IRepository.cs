namespace ImageProcessor.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity, TId> 
        where TEntity : class
        where TId: struct
    {
        Task<TEntity> GetById(TId id);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
