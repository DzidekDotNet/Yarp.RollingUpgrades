namespace Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

public class AllRule : RuleBase
{
    public override bool IsValid(IClusterChooserHttpContext httpContext)
    {
        return true;
    }
}