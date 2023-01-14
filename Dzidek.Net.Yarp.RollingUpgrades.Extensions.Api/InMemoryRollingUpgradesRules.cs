using System.Collections.Concurrent;
using Dzidek.Net.Yarp.RollingUpgrades.Rules;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api;

public class InMemoryRollingUpgradesRules
{
    internal ConcurrentBag<RollingUpgradesRule> Rules = new ConcurrentBag<RollingUpgradesRule>();
}