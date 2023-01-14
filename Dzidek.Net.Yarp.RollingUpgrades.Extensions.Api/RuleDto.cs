namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api;

/// <summary>
/// 
/// </summary>
/// <param name="RouteId">Rule route id</param>
/// <param name="ClusterName">Target cluster name</param>
/// <param name="Definition">Rule definition</param>
public readonly record struct RuleDto(string RouteId, string ClusterName, RuleDefinitionDto Definition);

/// <summary>
/// 
/// </summary>
/// <param name="RuleType">Rule class name</param>
/// <param name="DateFrom"></param>
/// <param name="DateTo"></param>
/// <param name="RuleValues">Rule constructor params</param>
public readonly record struct RuleDefinitionDto(string RuleType, DateTimeOffset? DateFrom, DateTimeOffset? DateTo, Dictionary<string, string> RuleValues);