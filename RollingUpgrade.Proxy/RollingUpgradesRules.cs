using Dzidek.Net.Yarp.RollingUpgrades.Rules;
using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

namespace RollingUpgrade.Proxy;

internal sealed class RollingUpgradesRules : IRollingUpgradesRulesQuery
{
    public IEnumerable<RollingUpgradesRule> GetRules()
    {
        return new List<RollingUpgradesRule>()
        {
            new RollingUpgradesRule("ApiRoute", "Api2", new HeaderRule("TenantId", "1"))
        };
    }
}