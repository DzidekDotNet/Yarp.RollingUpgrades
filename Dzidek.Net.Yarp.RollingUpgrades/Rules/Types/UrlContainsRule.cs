namespace Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

public class UrlContainsRule : RuleBase
{
    private readonly string _url;

    public UrlContainsRule(string url)
    {
        _url = url;
    }

    public override bool IsValid(IClusterChooserHttpContext httpContext)
    {
        return httpContext.Url.Contains(_url, StringComparison.InvariantCultureIgnoreCase);
    }
}