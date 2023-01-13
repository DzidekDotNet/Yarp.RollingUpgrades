namespace Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

public abstract class RuleBase
{
    public abstract bool IsValid(IClusterChooserHttpContext httpContext);
}