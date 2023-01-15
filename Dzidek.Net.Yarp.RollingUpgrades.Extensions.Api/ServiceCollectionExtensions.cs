using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddAllGenericTypes(this IServiceCollection services, Type t, Assembly[] assemblies,
        bool additionalRegisterTypesByThemself = false, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        var genericType = t;
        var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType)));

        foreach (var type in typesFromAssemblies)
        {
            foreach (var implementedInterface in type.ImplementedInterfaces)
            {
                switch (lifetime)
                {
                    case ServiceLifetime.Scoped:
                        services.AddScoped(implementedInterface, type);
                        break;
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(implementedInterface, type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(implementedInterface, type);
                        break;
                }
            }
        }

        return services;
    }
}