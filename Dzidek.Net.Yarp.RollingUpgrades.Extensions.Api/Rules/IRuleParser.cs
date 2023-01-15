using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api.Rules;

public interface IRuleParser<out T> where T : RuleBase
{
    T Parse(RuleDefinition rule);
}