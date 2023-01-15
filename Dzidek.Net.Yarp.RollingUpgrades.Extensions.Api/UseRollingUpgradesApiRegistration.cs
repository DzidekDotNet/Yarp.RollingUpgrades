using Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api.Rules;
using Dzidek.Net.Yarp.RollingUpgrades.Rules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api;

public static class UseRollingUpgradesApiRegistration
{
    public static IServiceCollection UseRollingUpgradesApi(this IServiceCollection services, string apiKey = "DzidekDotNet")
    {
        ApiKeyConfigurationInMemoryStorage.ApiKey = apiKey;
        services.AddControllers(options => options.Filters.Add<ApiKeyAttribute>());
        return services.AddSingleton<InMemoryRollingUpgradesRules>()
            .AddSingleton<IRollingUpgradesRulesQuery, RollingUpgradesRulesQuery>()
            .AddSingleton<IApiKeyConfiguration, ApiKeyConfiguration>()
            .AddRules();
    }
}