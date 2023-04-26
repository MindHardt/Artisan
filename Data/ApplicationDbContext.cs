using Artisan.Data.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace Artisan.Data;

public class ApplicationDbContext : 
    ApiAuthorizationDbContext<ApplicationUser>,
    IDataProtectionKeyContext
{

    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
    
    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        
        base.OnModelCreating(builder);
    }
}
