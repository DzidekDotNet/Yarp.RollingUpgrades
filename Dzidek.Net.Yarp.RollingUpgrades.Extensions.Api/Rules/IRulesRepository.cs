namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api.Rules;

public interface IRulesRepository
{
    void SaveRules(List<Rule> rules);
}