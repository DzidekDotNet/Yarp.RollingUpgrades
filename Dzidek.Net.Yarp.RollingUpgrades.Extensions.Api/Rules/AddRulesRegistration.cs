using Microsoft.Extensions.DependencyInjection;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api.Rules;

internal static class AddRulesRegistration
{
    internal static IServiceCollection AddRules(this IServiceCollection services)
    {
        return services
            .AddTransient<IRulesRepository, RulesRepository>()
            .AddAllGenericTypes(typeof(IRuleParser<>), AppDomain.CurrentDomain.GetAssemblies());
    }
}