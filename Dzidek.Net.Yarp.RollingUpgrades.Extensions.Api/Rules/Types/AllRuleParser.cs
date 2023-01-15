using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api.Rules.Types;

internal sealed class AllRuleParser : IRuleParser<AllRule>
{
    public AllRule Parse(RuleDefinition rule)
    {
        return new AllRule(rule.DateFrom, rule.DateTo);
    }
}