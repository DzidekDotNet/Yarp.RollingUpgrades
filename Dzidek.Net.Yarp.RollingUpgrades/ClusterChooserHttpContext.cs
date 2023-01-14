using Microsoft.Extensions.Primitives;

namespace Dzidek.Net.Yarp.RollingUpgrades;

internal readonly record struct ClusterChooserHttpContext(
    string Url, 
    IDictionary<string, StringValues> Headers,
    IEnumerable<KeyValuePair<string, string>> Cookies,
    IEnumerable<KeyValuePair<string, StringValues>> Form,
    string Method,
    string? ContentType,
    IEnumerable<KeyValuePair<string, StringValues>> Query) : IClusterChooserHttpContext;