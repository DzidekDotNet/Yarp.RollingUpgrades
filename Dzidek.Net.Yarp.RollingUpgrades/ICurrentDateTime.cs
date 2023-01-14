namespace Dzidek.Net.Yarp.RollingUpgrades;

public interface ICurrentDateTime
{
    DateTimeOffset GetDateTime();
}