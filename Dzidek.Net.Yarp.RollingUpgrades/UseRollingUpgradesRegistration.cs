using Dzidek.Net.Yarp.RollingUpgrades.Rules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy;

namespace Dzidek.Net.Yarp.RollingUpgrades;

public static class UseRollingUpgradesRegistration
{
    public static IApplicationBuilder UseRollingUpgrades(this IApplicationBuilder app, IRollingUpgradesRulesQuery rulesQuery)
    {
        IProxyStateLookup proxyStateLookup = app.ApplicationServices.GetRequiredService<IProxyStateLookup>();
        app.Use((context, next) =>
        {
            var clusterSelected = ClusterSelectors.ClusterSelector.ChooseCluster(context, rulesQuery);
            if (clusterSelected != null && proxyStateLookup.TryGetCluster(clusterSelected, out var cluster))
            {
                context.ReassignProxyRequest(cluster);
            }

            return next();
        });

        return app;
    }
}