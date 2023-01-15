using Dzidek.Net.Yarp.RollingUpgrades.Rules;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api.Rules;

internal class RollingUpgradesRulesQuery : IRollingUpgradesRulesQuery
{
    private readonly InMemoryRollingUpgradesRules _inMemoryRollingUpgradesRules;

    public RollingUpgradesRulesQuery(InMemoryRollingUpgradesRules inMemoryRollingUpgradesRules)
    {
        _inMemoryRollingUpgradesRules = inMemoryRollingUpgradesRules;
    }

    public IEnumerable<RollingUpgradesRule> GetRules()
    {
        return _inMemoryRollingUpgradesRules.Rules;
    }
}