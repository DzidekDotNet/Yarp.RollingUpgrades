using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

namespace Dzidek.Net.Yarp.RollingUpgrades.Rules;

public readonly record struct RollingUpgradesRule(string RouteId, string ClusterName,RuleBase Rule): IRollingUpgradesRule;