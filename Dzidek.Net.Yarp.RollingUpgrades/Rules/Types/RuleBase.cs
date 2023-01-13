namespace Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

public abstract class RuleBase
{
    internal abstract bool IsValid(IClusterChooserHttpContext httpContext);
}