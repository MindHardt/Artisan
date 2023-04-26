using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Artisan.Data.Repos;

public static class DependencyInjection
{
    /// <summary>
    /// Reflectively finds all the inheritors of <see cref="RepositoryBase{T}"/>
    /// and adds them to <paramref name="services"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="additionalAssemblies">Optional additional assemblies to get types from.</param>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddRepositories<TContext>(
        this IServiceCollection services, 
        IEnumerable<Assembly>? additionalAssemblies = null)
        where TContext : DbContext
    {
        var repoTypes = GetRepoTypes();
        
        services.AddScoped<DbContext, TContext>();
        foreach (Type repoType in repoTypes)
        {
            services.AddScoped(repoType);
        }

        return services;
    }

    private static IEnumerable<Type> GetRepoTypes(IEnumerable<Assembly>? additionalAssemblies = null)
    {
        additionalAssemblies ??= Enumerable.Empty<Assembly>();
        
        return additionalAssemblies
            .Append(typeof(RepositoryBase<>).Assembly)
            .SelectMany(asm => asm.GetTypes())
            .Where(IsRepoType);
    }

    private static bool IsRepoType(Type type) =>
        type.IsAbstract is false &&
        type.BaseType is not null &&
        type.BaseType.IsGenericType &&
        type.BaseType.GetGenericTypeDefinition() == typeof(RepositoryBase<>);
}