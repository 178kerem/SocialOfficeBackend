using Microsoft.EntityFrameworkCore;
using SocialOffice.Application.Interfaces.Persistence.Common;

using SocialOffice.Domain.Entitites.Shared;
using SocialOffice.Infrastructure.Persistence;
using System.Linq.Expressions;
namespace SocialOffice.Infrastructure.Persistence.Repositories.Common;
public class Repository<T> : IAsyncRepository<T> where T : BaseEntity
{
    protected readonly SOfficeDbContext _dbContext;

    public Repository(SOfficeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>()
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>()
            .Where(x => !x.IsDeleted)
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);
        return entity != null && !entity.IsDeleted ? entity : null;
    }

    public async Task<T> AddAsync(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        // Soft Delete
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
}
