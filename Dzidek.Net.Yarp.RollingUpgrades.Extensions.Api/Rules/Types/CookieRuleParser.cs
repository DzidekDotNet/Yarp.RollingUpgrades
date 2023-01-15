using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api.Rules.Types;

internal sealed class CookieRuleParser : IRuleParser<CookieRule>
{
    public CookieRule Parse(RuleDefinition rule)
    {
        return new CookieRule(rule.RuleValues["cookieName"], rule.RuleValues["cookieValue"], rule.DateFrom,
            rule.DateTo);
    }
}