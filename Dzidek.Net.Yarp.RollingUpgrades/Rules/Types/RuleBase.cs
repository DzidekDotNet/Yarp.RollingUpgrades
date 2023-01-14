namespace Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;

public abstract class RuleBase
{
    private readonly DateTimeOffset? _dateFrom;
    private readonly DateTimeOffset? _dateTo;

    protected RuleBase() : this(null, null)
    {
        
    }
    protected RuleBase(DateTimeOffset? dateFrom, DateTimeOffset? dateTo)
    {
        _dateFrom = dateFrom;
        _dateTo = dateTo;
    }
    public abstract bool IsValid(IClusterChooserHttpContext httpContext);

    internal bool IsRuleValid(IClusterChooserHttpContext httpContext, ICurrentDateTime currentDateTime)
    {
        return IsDateFromIsValid(_dateFrom, currentDateTime.GetDateTime())
               && IsDateToIsValid(_dateTo, currentDateTime.GetDateTime())
               && IsValid(httpContext);
    }
    
    private bool IsDateFromIsValid(DateTimeOffset? dateFrom, DateTimeOffset currentDate)
    {
        return !_dateFrom.HasValue || currentDate >= dateFrom;
    }
    private bool IsDateToIsValid(DateTimeOffset? dateTo, DateTimeOffset currentDate)
    {
        return !dateTo.HasValue || currentDate <= dateTo;
    }
}