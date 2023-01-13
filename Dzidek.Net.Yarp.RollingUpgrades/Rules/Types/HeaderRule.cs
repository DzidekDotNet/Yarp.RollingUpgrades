namespace Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

public class HeaderRule : RuleBase
{
    private readonly string _headerName;
    private readonly string _headerValue;

    public HeaderRule(string headerName, string headerValue)
    {
        _headerName = headerName;
        _headerValue = headerValue;
    }

    internal override bool IsValid(IClusterChooserHttpContext httpContext)
    {
        return httpContext.Headers.ContainsKey(_headerName) && httpContext.Headers[_headerName].Contains(_headerValue);
    }
}