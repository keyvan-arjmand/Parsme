using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext DbContext;

    public Repository(DbSet<T> entities, IQueryable<T> table, IQueryable<T> tableNoTracking, DbContext dbContext)
    {
        DbContext = dbContext;
        Entities = DbContext.Set<T>();
    }
  
    public DbSet<T> Entities { get; }
    public virtual IQueryable<T> Table => Entities.AsNoTracking();
    public virtual IQueryable<T> TableNoTracking => Entities.AsNoTracking();

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        Assert.NotNull(entity);
        await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        Assert.NotNull(entities);
        await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        Assert.NotNull(entity);
        Entities.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async ValueTask<T> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return await Entities.FindAsync(ids, cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        Assert.NotNull(entity);
        Entities.Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}