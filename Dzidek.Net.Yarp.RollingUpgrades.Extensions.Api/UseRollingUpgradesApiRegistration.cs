using Dzidek.Net.Yarp.RollingUpgrades.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api;

public static class UseRollingUpgradesApiRegistration
{
    public static IServiceCollection UseRollingUpgradesApi(this IServiceCollection services)
    {
        return services.AddSingleton<InMemoryRollingUpgradesRules>()
            .AddSingleton<IRollingUpgradesRulesQuery, RollingUpgradesRulesQuery>();
    }
}