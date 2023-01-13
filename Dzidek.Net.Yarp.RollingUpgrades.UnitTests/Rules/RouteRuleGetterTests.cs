using Dzidek.Net.Yarp.RollingUpgrades.Rules;
using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;
using FluentAssertions;
using Moq;

namespace Dzidek.Net.Yarp.RollingUpgrades.UnitTests.Rules;

public class RouteRuleGetterTests
{
    [Theory]
    [ClassData(typeof(RouteRuleGetterData))]
    public void GetRulesForRoute_ShouldReturnProperRulesForRouteId(TestCaseInput testCaseInput,
        TestCaseExpected testCaseExpected)
    {
        var testee = new RouteRuleGetter(testCaseInput.RulesQuery);
        var result = testee.GetRulesForRoute(testCaseInput.RouteId);

        result.Should().BeEquivalentTo(testCaseExpected.ExpectedRules);
    }
}

public record struct TestCaseInput(IRollingUpgradesRulesQuery RulesQuery, string RouteId);

public record struct TestCaseExpected(IEnumerable<RollingUpgradesRule> ExpectedRules);

internal sealed class RouteRuleGetterData : TheoryData<TestCaseInput, TestCaseExpected>
{
    public RouteRuleGetterData()
    {
        var rule = new HeaderRule("testHeader", "1");
        var rulesQuery = new Mock<IRollingUpgradesRulesQuery>();
        rulesQuery.Setup(x => x.GetRules())
            .Returns(new List<RollingUpgradesRule>()
            {
                new RollingUpgradesRule("api", "cluster1", rule),
                new RollingUpgradesRule("ui", "cluster1", rule),
                new RollingUpgradesRule("identity", "cluster1", rule),
            });
        Add(new TestCaseInput(rulesQuery.Object, "api"),
            new TestCaseExpected(new List<RollingUpgradesRule>()
            {
                new RollingUpgradesRule("api", "cluster1", rule),
            }));
        Add(new TestCaseInput(rulesQuery.Object, "ui"),
            new TestCaseExpected(new List<RollingUpgradesRule>()
            {
                new RollingUpgradesRule("ui", "cluster1", rule),
            }));
    }
}