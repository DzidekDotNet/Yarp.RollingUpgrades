namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api;

internal sealed class ApiKeyConfiguration: IApiKeyConfiguration
{
    public string ApiKey => ApiKeyConfigurationInMemoryStorage.ApiKey;
}

internal static class ApiKeyConfigurationInMemoryStorage
{
    public static string ApiKey { get; set; }
}