using Microsoft.Extensions.Primitives;

namespace Dzidek.Net.Yarp.RollingUpgrades;

public interface IClusterChooserHttpContext
{
    string Url { get; }
    string Method { get; }
    string? ContentType { get; }
    IDictionary<string, StringValues> Headers { get; }
    IEnumerable<KeyValuePair<string, string>> Cookies { get; }
    IEnumerable<KeyValuePair<string, StringValues>> Form { get; }
    IEnumerable<KeyValuePair<string, StringValues>> Query { get; }
}