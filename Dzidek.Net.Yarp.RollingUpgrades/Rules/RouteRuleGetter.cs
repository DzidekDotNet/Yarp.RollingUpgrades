namespace Dzidek.Net.Yarp.RollingUpgrades.Rules;

internal sealed class RouteRuleGetter : IRouteRuleGetter
{
    private readonly IRollingUpgradesRulesQuery _rulesQuery;

    internal RouteRuleGetter(IRollingUpgradesRulesQuery rulesQuery)
    {
        _rulesQuery = rulesQuery;
    }

    public IEnumerable<IRollingUpgradesRule> GetRulesForRoute(string routeId)
    {
        return (_rulesQuery.GetRules()).Where(x => x.RouteId == routeId).Select(x => x as IRollingUpgradesRule);
    }
}