using Dzidek.Net.Yarp.RollingUpgrades;
using Dzidek.Net.Yarp.RollingUpgrades.Rules;
using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

namespace RollingUpgrade.Proxy;

internal sealed class RollingUpgradesRules : IRollingUpgradesRulesQuery
{
    public IEnumerable<RollingUpgradesRule> GetRules()
    {
        return new List<RollingUpgradesRule>()
        {
            new RollingUpgradesRule("ApiRoute", "Api2", new HeaderRule("TenantId", "1"))
        };
    }
}

// Example of custom rule

// internal sealed class RollingUpgradesRules : IRollingUpgradesRulesQuery
// {
//     public IEnumerable<RollingUpgradesRule> GetRules()
//     {
//         return new List<RollingUpgradesRule>()
//         {
//             new RollingUpgradesRule("ApiRoute", "Api2", new CustomRule("TenantId", "1", "OtherHeader", "1"))
//         };
//     }
// }
//
public class CustomRule : RuleBase
{
    private readonly string _headerName1;
    private readonly string _headerValue1;
    private readonly string _headerName2;
    private readonly string _headerValue2;

    public CustomRule(string headerName1, string headerValue1, string headerName2, string headerValue2)
    {
        _headerName1 = headerName1;
        _headerValue1 = headerValue1;
        _headerName2 = headerName2;
        _headerValue2 = headerValue2;
    }

    public override bool IsValid(IClusterChooserHttpContext httpContext)
    {
        return httpContext.Headers.ContainsKey(_headerName1) && httpContext.Headers[_headerName1].Contains(_headerValue1)
            && httpContext.Headers.ContainsKey(_headerName2) && httpContext.Headers[_headerName2].Contains(_headerValue2);
    }
}