namespace Dzidek.Net.Yarp.RollingUpgrades;

internal sealed class CurrentDateTime: ICurrentDateTime
{
    public DateTimeOffset GetDateTime()
    {
        return DateTimeOffset.UtcNow;
    }
}