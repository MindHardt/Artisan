using Microsoft.EntityFrameworkCore;

namespace Artisan.Data.Repos;

/// <summary>
/// Represents abstract repository with underlying <see cref="ApplicationDbContext"/>.
/// </summary>
public abstract class RepositoryBase<T> 
    where T : class
{
    protected RepositoryBase(DbContext context)
    {
        Context = context;
    }

    protected DbContext Context { get; }
    protected DbSet<T> Set => Context.Set<T>();
    
    public Task CommitAsync() => Context.SaveChangesAsync();

}