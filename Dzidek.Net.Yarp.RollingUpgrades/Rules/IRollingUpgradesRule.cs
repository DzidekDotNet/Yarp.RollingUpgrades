using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

namespace Dzidek.Net.Yarp.RollingUpgrades.Rules;

internal interface IRollingUpgradesRule
{
    string RouteId { get; }
    string ClusterName { get; }
    RuleBase Rule { get; }
}