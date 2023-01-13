namespace Dzidek.Net.Yarp.RollingUpgrades.Rules;

internal interface IRouteRuleGetter
{
    IEnumerable<IRollingUpgradesRule> GetRulesForRoute(string routeId);
}