using Microsoft.Extensions.Primitives;

namespace Dzidek.Net.Yarp.RollingUpgrades;

public interface IClusterChooserHttpContext
{
    string Url { get; }
    IDictionary<string, StringValues> Headers { get; }
    IEnumerable<KeyValuePair<string, string>> Cookies { get; }
}