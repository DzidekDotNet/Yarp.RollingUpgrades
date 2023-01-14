using Dzidek.Net.Yarp.RollingUpgrades.Rules;

namespace Dzidek.Net.Yarp.RollingUpgrades.ClusterSelectors;

internal interface IRuleBasedClusterChooser
{
    string? GetClusterName(IEnumerable<IRollingUpgradesRule> rules, IClusterChooserHttpContext httpContext, ICurrentDateTime currentDateTime);
}