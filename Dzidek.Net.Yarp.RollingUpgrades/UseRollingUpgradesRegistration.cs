using Dzidek.Net.Yarp.RollingUpgrades.ClusterSelectors;
using Dzidek.Net.Yarp.RollingUpgrades.Rules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy;

namespace Dzidek.Net.Yarp.RollingUpgrades;

public static class UseRollingUpgradesRegistration
{
    public static IApplicationBuilder UseRollingUpgrades(this IApplicationBuilder app,
        IRollingUpgradesRulesQuery? rulesQuery = null)
    {
        IProxyStateLookup proxyStateLookup = app.ApplicationServices.GetRequiredService<IProxyStateLookup>();
        app.Use((context, next) =>
        {
            var clusterSelected = ClusterSelector.ChooseCluster(context, rulesQuery??app.ApplicationServices.GetRequiredService<IRollingUpgradesRulesQuery>());
            if (clusterSelected != null && proxyStateLookup.TryGetCluster(clusterSelected, out var cluster) &&
                cluster.DestinationsState.AvailableDestinations.Any())
            {
                context.ReassignProxyRequest(cluster);
            }

            return next();
        });

        return app;
    }
}