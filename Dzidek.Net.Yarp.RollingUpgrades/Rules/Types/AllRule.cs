namespace Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

public class AllRule : RuleBase
{
    public AllRule(DateTimeOffset? dateFrom = null, DateTimeOffset? dateTo = null) : base(
        dateFrom, dateTo)
    {
    }

    public override bool IsValid(IClusterChooserHttpContext httpContext)
    {
        return true;
    }
}