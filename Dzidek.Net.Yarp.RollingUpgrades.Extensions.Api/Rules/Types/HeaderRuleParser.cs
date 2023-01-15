using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api.Rules.Types;

internal sealed class HeaderRuleParser : IRuleParser<HeaderRule>
{
    public HeaderRule Parse(RuleDefinition rule)
    {
        return new HeaderRule(rule.RuleValues["headerName"], rule.RuleValues["headerValue"], rule.DateFrom,
            rule.DateTo);
    }
}