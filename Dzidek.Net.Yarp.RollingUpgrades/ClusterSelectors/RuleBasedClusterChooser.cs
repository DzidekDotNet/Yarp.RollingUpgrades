using Dzidek.Net.Yarp.RollingUpgrades.Rules;

namespace Dzidek.Net.Yarp.RollingUpgrades.ClusterSelectors;

internal sealed class RuleBasedClusterChooser : IRuleBasedClusterChooser
{
    public string? GetClusterName(IEnumerable<IRollingUpgradesRule> rules, IClusterChooserHttpContext httpContext,
        ICurrentDateTime currentDateTime)
    {
        foreach (var rule in rules)
        {
            if (rule.Rule.IsRuleValid(httpContext, currentDateTime))
            {
                return rule.ClusterName;
            }
        }

        return null;
    }
}