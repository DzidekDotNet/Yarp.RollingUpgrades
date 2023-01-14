namespace Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

public class CookieRule : RuleBase
{
    private readonly string _cookieName;
    private readonly string _cookieValue;

    public CookieRule(string cookieName, string cookieValue, DateTimeOffset? dateFrom = null, DateTimeOffset? dateTo = null) : base(
        dateFrom, dateTo)
    {
        _cookieName = cookieName;
        _cookieValue = cookieValue;
    }

    public override bool IsValid(IClusterChooserHttpContext httpContext)
    {
        return httpContext.Cookies.Any(x => x.Key == _cookieName && x.Value == _cookieValue);
    }
}