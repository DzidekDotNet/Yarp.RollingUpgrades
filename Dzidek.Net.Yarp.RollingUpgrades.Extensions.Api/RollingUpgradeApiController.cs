using Dzidek.Net.Yarp.RollingUpgrades.Rules;
using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api;

[ApiController]
[Route("[controller]")]
[Host("localhost")]
public class RollingUpgradeApiController : ControllerBase
{
    private readonly InMemoryRollingUpgradesRules _inMemoryRollingUpgradesRules;

    public RollingUpgradeApiController(InMemoryRollingUpgradesRules inMemoryRollingUpgradesRules)
    {
        _inMemoryRollingUpgradesRules = inMemoryRollingUpgradesRules;
    }

    [HttpGet]
    public Task<OkObjectResult> Get()
    {
        return Task.FromResult(Ok(_inMemoryRollingUpgradesRules.Rules.ToList()));
    }

    [HttpPost]
    public Task Set(List<RuleDto> rules)
    {
        List<RollingUpgradesRule> newRules = GetRollingUpgradesRules(rules);
        _inMemoryRollingUpgradesRules.Rules.Clear();
        newRules.ForEach(x => _inMemoryRollingUpgradesRules.Rules.Add(x));
        return Task.FromResult(Ok());
    }

    private List<RollingUpgradesRule> GetRollingUpgradesRules(List<RuleDto> rules)
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

    private RollingUpgradesRule? CreateRule(RuleDto ruleDto)
    {
        RuleBase? rule = CreateRuleBase(ruleDto.Definition);
        if (rule != null)
        {
            return new RollingUpgradesRule(ruleDto.RouteId, ruleDto.ClusterName, rule);
        }

        return null;
    }

    /// <summary>
    /// TODO: Do it better :)
    /// </summary>
    /// <param name="ruleDefinitionDto"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private RuleBase? CreateRuleBase(RuleDefinitionDto ruleDefinitionDto)
    {
        Type? ruleType = GetTypeByName(ruleDefinitionDto.RuleType);
        if (ruleType == null)
        {
            throw new Exception("Configuration error");
        }

        var constructors = ruleType.GetConstructors();
        var constructor = constructors.First(x => x.GetParameters().Length == ruleDefinitionDto.RuleValues.Count + 2 || x.GetParameters().Length == ruleDefinitionDto.RuleValues.Count);
        var parameters = constructor.GetParameters();
        object?[] constructorParameters = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            if ("datefrom" == parameters[i].Name.ToLower())
            {
                constructorParameters[i] = ruleDefinitionDto.DateFrom;
            }

            if ("dateto" == parameters[i].Name.ToLower())
            {
                constructorParameters[i] = ruleDefinitionDto.DateTo;
            }

            if (!new[] { "dateto", "datefrom" }.Contains(parameters[i].Name.ToLower()))
            {
                constructorParameters[i] = ruleDefinitionDto.RuleValues[parameters[i].Name];
            }
        }

        return constructor.Invoke(constructorParameters) as RuleBase;
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