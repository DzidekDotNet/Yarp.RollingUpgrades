using Dzidek.Net.Yarp.RollingUpgrades.Rules;
using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api.Rules;

internal sealed class RulesRepository: IRulesRepository
{
    private readonly InMemoryRollingUpgradesRules _inMemoryRollingUpgradesRules;
    private readonly IServiceProvider _serviceProvider;

    public RulesRepository(InMemoryRollingUpgradesRules inMemoryRollingUpgradesRules,
        IServiceProvider serviceProvider)
    {
        _inMemoryRollingUpgradesRules = inMemoryRollingUpgradesRules;
        _serviceProvider = serviceProvider;
    }
    public void SaveRules(List<Rule> rules)
    {
        List<RollingUpgradesRule> newRules = GetRollingUpgradesRules(rules);
        _inMemoryRollingUpgradesRules.Rules.Clear();
        newRules.ForEach(x => _inMemoryRollingUpgradesRules.Rules.Add(x));
    }
    
    private List<RollingUpgradesRule> GetRollingUpgradesRules(List<Rule> rules)
    {
        List<RollingUpgradesRule> newRules = new List<RollingUpgradesRule>(rules.Count);
        rules.ForEach(rule =>
        {
            RollingUpgradesRule? ruleToAdd = CreateRule(rule);
            if (ruleToAdd != null)
            {
                newRules.Add(ruleToAdd.Value);
            }
        });
        return newRules;
    }

    private RollingUpgradesRule? CreateRule(Rule rule)
    {
        RuleBase? ruleBase = CreateRuleBase(rule.Definition);
        if (ruleBase != null)
        {
            return new RollingUpgradesRule(rule.RouteId, rule.ClusterName, ruleBase);
        }

        return null;
    }

    private RuleBase? CreateRuleBase(RuleDefinition ruleDefinition)
    {
        Type? ruleType = GetTypeByName(ruleDefinition.RuleType);
        if (ruleType == null)
        {
            throw new Exception("Configuration error");
        }

        var handlerType = typeof(IRuleParser<>)
            .MakeGenericType(ruleType);
        var ruleParser = _serviceProvider.GetRequiredService(handlerType) as IRuleParser<RuleBase>;
        return ruleParser.Parse(ruleDefinition);
    }

    private Type? GetTypeByName(string name)
    {
        return
            AppDomain.CurrentDomain.GetAssemblies()
                .Reverse()
                .Select(assembly => assembly.GetType(name))
                .FirstOrDefault(t => t != null)
            // Safely delete the following part
            // if you do not want fall back to first partial result
            ??
            AppDomain.CurrentDomain.GetAssemblies()
                .Reverse()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(t => t.Name.Contains(name));
    }
}