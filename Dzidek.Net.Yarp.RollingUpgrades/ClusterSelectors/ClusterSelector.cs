using Dzidek.Net.Yarp.RollingUpgrades.Rules;
using Microsoft.AspNetCore.Http;
using Yarp.ReverseProxy.Model;

namespace Dzidek.Net.Yarp.RollingUpgrades.ClusterSelectors;

internal static class ClusterSelector
{
    internal static string? ChooseCluster(HttpContext context, IRollingUpgradesRulesQuery rulesQuery)
    {
        IRouteRuleGetter routeRuleGetter = new RouteRuleGetter(rulesQuery);
        IReverseProxyFeature reverseProxyFeature = context.GetReverseProxyFeature();
        var routeRules = routeRuleGetter.GetRulesForRoute(reverseProxyFeature.Route.Config.RouteId);
        IRuleBasedClusterChooser ruleBasedClusterChooser = new RuleBasedClusterChooser();
        return ruleBasedClusterChooser.GetClusterName(routeRules, GetHttpContext(context), new CurrentDateTime());
    }

    private static IClusterChooserHttpContext GetHttpContext(HttpContext context)
    {
        return new ClusterChooserHttpContext(
            context.Request.Host.ToString(), 
            context.Request.Headers, 
            context.Request.Cookies,
            context.Request.Method,
            context.Request.Query);
    }
}