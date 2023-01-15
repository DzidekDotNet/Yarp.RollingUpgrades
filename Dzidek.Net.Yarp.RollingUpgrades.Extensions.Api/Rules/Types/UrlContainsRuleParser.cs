using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api.Rules.Types;

internal sealed class UrlContainsRuleParser : IRuleParser<UrlContainsRule>
{
    public UrlContainsRule Parse(RuleDefinition rule)
    {
        return new UrlContainsRule(rule.RuleValues["url"], rule.DateFrom, rule.DateTo);
    }
}