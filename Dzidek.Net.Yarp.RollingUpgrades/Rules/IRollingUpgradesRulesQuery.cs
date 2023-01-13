namespace Dzidek.Net.Yarp.RollingUpgrades.Rules;

public interface IRollingUpgradesRulesQuery
{
    IEnumerable<RollingUpgradesRule> GetRules();
}