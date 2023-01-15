using Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api.Rules;
using Microsoft.AspNetCore.Mvc;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api;

[ApiController]
[Route("[controller]")]
[ApiKey]
public class RollingUpgradeApiController : ControllerBase
{
    private readonly InMemoryRollingUpgradesRules _inMemoryRollingUpgradesRules;
    private readonly IRulesRepository _rulesRepository;

    public RollingUpgradeApiController(InMemoryRollingUpgradesRules inMemoryRollingUpgradesRules,
        IRulesRepository rulesRepository)
    {
        _inMemoryRollingUpgradesRules = inMemoryRollingUpgradesRules;
        _rulesRepository = rulesRepository;
    }

    [HttpGet]
    public Task<OkObjectResult> Get()
    {
        return Task.FromResult(Ok(_inMemoryRollingUpgradesRules.Rules.ToList()));
    }

    [HttpPost]
    public Task Set(List<Rule> rules)
    {
        _rulesRepository.SaveRules(rules);
        return Task.FromResult(Ok());
    }
}