namespace Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

public class AllRule : RuleBase
{
    internal override bool IsValid(IClusterChooserHttpContext httpContext)
    {
        return true;
    }
}