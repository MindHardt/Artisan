using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Artisan.CommonComponents.JsInterop;

public static class DependencyInjection
{
    /// <summary>
    /// Reflectively searches for all derivatives of <see cref="JsInteropBase"/>
    /// and adds them to <paramref name="services"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddJsInterops(this IServiceCollection services)
    {
        var assemblies = GetAllAssemblies();

        var interops = assemblies
            .SelectMany(GetJsInterops)
            .ToArray();
        
        // Console.WriteLine(string.Join('\n', assemblies));
        // Console.WriteLine(string.Join('\n', interops.AsEnumerable()));

        foreach (Type interop in interops)
        {
            services.AddScoped(interop);
        }

        return services;
    }
    
    private static IEnumerable<Type> GetJsInterops(Assembly assembly) => assembly
        .GetTypes()
        .Where(t => t.IsAssignableTo(typeof(JsInteropBase)) && t.IsAbstract is false);

    private static ICollection<Assembly> GetAllAssemblies()
    {
        var primaryAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(asm => IsArtisanAssembly(asm.GetName()))
            .ToArray();

        var totalAssemblies = new HashSet<Assembly>(primaryAssemblies);

        foreach (Assembly primaryAssembly in primaryAssemblies)
        {
            RecursiveAddReferencedAssemblies(primaryAssembly, totalAssemblies);
        }

        return totalAssemblies;
    }

    private static void RecursiveAddReferencedAssemblies(Assembly assembly, ISet<Assembly> collection)
    {
        foreach (AssemblyName asmName in assembly.GetReferencedAssemblies())
        {
            if (IsArtisanAssembly(asmName) is false)
                continue;
            
            Assembly asm = Assembly.Load(asmName);
            if (collection.Add(asm))
            {
                RecursiveAddReferencedAssemblies(asm, collection);
            }
        }
    }

    private static bool IsArtisanAssembly(AssemblyName asmName) => asmName.FullName.Contains("Artisan");
}