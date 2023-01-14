using Dzidek.Net.Yarp.RollingUpgrades.ClusterSelectors;
using Dzidek.Net.Yarp.RollingUpgrades.Rules;
using Dzidek.Net.Yarp.RollingUpgrades.Rules.Types;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Moq;

namespace Dzidek.Net.Yarp.RollingUpgrades.UnitTests.ClusterSelector;

public class RuleBasedClusterChooserTests
{
    [Theory]
    [ClassData(typeof(AllRuleData))]
    public void GetClusterName_WhenAllRule_ShouldReturnProperClusterName(TestCaseInput testCaseInput,
        string? expectedClusterName) =>
        GetClusterName_ShouldReturnProperClusterName(testCaseInput, expectedClusterName);

    [Theory]
    [ClassData(typeof(HeaderRuleData))]
    public void GetClusterName_WhenHeaderRule_ShouldReturnProperClusterName(TestCaseInput testCaseInput,
        string? expectedClusterName) =>
        GetClusterName_ShouldReturnProperClusterName(testCaseInput, expectedClusterName);

    [Theory]
    [ClassData(typeof(CookieRuleData))]
    public void GetClusterName_WhenCookieRule_ShouldReturnProperClusterName(TestCaseInput testCaseInput,
        string? expectedClusterName) =>
        GetClusterName_ShouldReturnProperClusterName(testCaseInput, expectedClusterName);

    [Theory]
    [ClassData(typeof(UrlContainsRuleData))]
    public void GetClusterName_WhenUrlContainsRule_ShouldReturnProperClusterName(TestCaseInput testCaseInput,
        string? expectedClusterName) =>
        GetClusterName_ShouldReturnProperClusterName(testCaseInput, expectedClusterName);

    [Theory]
    [ClassData(typeof(ScheduledAllRuleData))]
    public void GetClusterName_WhenScheduledAllRule_ShouldReturnProperClusterName(TestCaseInput testCaseInput,
        string? expectedClusterName) =>
        GetClusterName_ShouldReturnProperClusterName(testCaseInput, expectedClusterName);
    
    [Theory]
    [ClassData(typeof(ScheduledHeaderRuleData))]
    public void GetClusterName_WhenScheduledHeaderRule_ShouldReturnProperClusterName(TestCaseInput testCaseInput,
        string? expectedClusterName) =>
        GetClusterName_ShouldReturnProperClusterName(testCaseInput, expectedClusterName);

    private void GetClusterName_ShouldReturnProperClusterName(TestCaseInput testCaseInput,
        string? expectedClusterName)
    {
        var testee = new RuleBasedClusterChooser();
        var result = testee.GetClusterName(testCaseInput.Rules.Select(x => x as IRollingUpgradesRule),
            testCaseInput.HttpContext, testCaseInput.CurrentDateTime);

        result.Should().Be(expectedClusterName);
    }
}

public record struct TestCaseInput(IEnumerable<RollingUpgradesRule> Rules, IClusterChooserHttpContext HttpContext,
    ICurrentDateTime CurrentDateTime);

internal sealed class AllRuleData : TheoryData<TestCaseInput, string?>
{
    public AllRuleData()
    {
        var rule = new AllRule();
        var rules = new List<RollingUpgradesRule>()
        {
            new("api", "cluster1", rule),
        };
        var httpContext = new Mock<IClusterChooserHttpContext>();
        var currentDateTime = new Mock<ICurrentDateTime>();

        Add(new TestCaseInput(rules, httpContext.Object, currentDateTime.Object), "cluster1");
    }
}

internal sealed class HeaderRuleData : TheoryData<TestCaseInput, string?>
{
    public HeaderRuleData()
    {
        var rule = new HeaderRule("TenantId", "1");
        var rules = new List<RollingUpgradesRule>()
        {
            new("api", "cluster1", rule),
        };
        var httpContextTenant1 = new Mock<IClusterChooserHttpContext>();
        httpContextTenant1.SetupGet(x => x.Headers).Returns(new Dictionary<string, StringValues>()
        {
            { "TenantId", new StringValues("1") }
        });
        var httpContextTenant2 = new Mock<IClusterChooserHttpContext>();
        httpContextTenant2.SetupGet(x => x.Headers).Returns(new Dictionary<string, StringValues>()
        {
            { "TenantId", new StringValues("2") }
        });
        var httpContextWithoutTenant = new Mock<IClusterChooserHttpContext>();
        httpContextWithoutTenant.SetupGet(x => x.Headers).Returns(new Dictionary<string, StringValues>());
        var currentDateTime = new Mock<ICurrentDateTime>();

        Add(new TestCaseInput(rules, httpContextTenant1.Object, currentDateTime.Object), "cluster1");
        Add(new TestCaseInput(rules, httpContextTenant2.Object, currentDateTime.Object), null);
        Add(new TestCaseInput(rules, httpContextWithoutTenant.Object, currentDateTime.Object), null);
    }
}

internal sealed class CookieRuleData : TheoryData<TestCaseInput, string?>
{
    public CookieRuleData()
    {
        var rule = new CookieRule("TenantId", "1");
        var rules = new List<RollingUpgradesRule>()
        {
            new("api", "cluster1", rule),
        };
        var httpContextTenant1 = new Mock<IClusterChooserHttpContext>();
        httpContextTenant1.SetupGet(x => x.Cookies).Returns(new List<KeyValuePair<string, string>>()
        {
            new("TenantId", "1")
        });
        var httpContextTenant2 = new Mock<IClusterChooserHttpContext>();
        httpContextTenant2.SetupGet(x => x.Cookies).Returns(new List<KeyValuePair<string, string>>()
        {
            new("TenantId", "2")
        });
        var httpContextWithoutTenant = new Mock<IClusterChooserHttpContext>();
        httpContextWithoutTenant.SetupGet(x => x.Cookies).Returns(new List<KeyValuePair<string, string>>());
        var currentDateTime = new Mock<ICurrentDateTime>();

        Add(new TestCaseInput(rules, httpContextTenant1.Object, currentDateTime.Object), "cluster1");
        Add(new TestCaseInput(rules, httpContextTenant2.Object, currentDateTime.Object), null);
        Add(new TestCaseInput(rules, httpContextWithoutTenant.Object, currentDateTime.Object), null);
    }
}

internal sealed class UrlContainsRuleData : TheoryData<TestCaseInput, string?>
{
    public UrlContainsRuleData()
    {
        var rule = new UrlContainsRule("Tenant1");
        var rules = new List<RollingUpgradesRule>()
        {
            new("api", "cluster1", rule),
        };
        var httpContextTenant1 = new Mock<IClusterChooserHttpContext>();
        httpContextTenant1.SetupGet(x => x.Url)
            .Returns("https://tenant1.dzidek.net");
        var httpContextTenant2 = new Mock<IClusterChooserHttpContext>();
        httpContextTenant2.SetupGet(x => x.Url)
            .Returns("https://tenant2.dzidek.net");
        var currentDateTime = new Mock<ICurrentDateTime>();

        Add(new TestCaseInput(rules, httpContextTenant1.Object, currentDateTime.Object), "cluster1");
        Add(new TestCaseInput(rules, httpContextTenant2.Object, currentDateTime.Object), null);
    }
}

internal sealed class ScheduledAllRuleData : TheoryData<TestCaseInput, string?>
{
    public ScheduledAllRuleData()
    {
        var httpContextTenant1 = new Mock<IClusterChooserHttpContext>();
        httpContextTenant1.SetupGet(x => x.Headers).Returns(new Dictionary<string, StringValues>()
        {
            { "TenantId", new StringValues("1") }
        });
        var currentDateTime = new Mock<ICurrentDateTime>();
        currentDateTime.Setup(x => x.GetDateTime())
            .Returns(new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 2)));

        Add(new TestCaseInput(new List<RollingUpgradesRule>()
        {
            new("api", "cluster1", new AllRule(new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 1)), null)),
        }, httpContextTenant1.Object, currentDateTime.Object), "cluster1");
        Add(new TestCaseInput(new List<RollingUpgradesRule>()
        {
            new("api", "cluster1", new AllRule(new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 2)), null)),
        }, httpContextTenant1.Object, currentDateTime.Object), "cluster1");
        Add(new TestCaseInput(new List<RollingUpgradesRule>()
        {
            new("api", "cluster1", new AllRule(new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 3)), null)),
        }, httpContextTenant1.Object, currentDateTime.Object), null);
        Add(new TestCaseInput(new List<RollingUpgradesRule>()
        {
            new("api", "cluster1",
                new AllRule(new DateTimeOffset(new DateTime(2022, 1, 1, 1, 1, 1)), new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 2)))),
        }, httpContextTenant1.Object, currentDateTime.Object), "cluster1");
        Add(new TestCaseInput(new List<RollingUpgradesRule>()
        {
            new("api", "cluster1",
                new AllRule(new DateTimeOffset(new DateTime(2022, 1, 1, 1, 1, 2)), new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 3)))),
        }, httpContextTenant1.Object, currentDateTime.Object), "cluster1");
        Add(new TestCaseInput(new List<RollingUpgradesRule>()
        {
            new("api", "cluster1",
                new AllRule(new DateTimeOffset(new DateTime(2022, 1, 1, 1, 1, 3)), new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 1)))),
        }, httpContextTenant1.Object, currentDateTime.Object), null);
    }
}

internal sealed class ScheduledHeaderRuleData : TheoryData<TestCaseInput, string?>
{
    public ScheduledHeaderRuleData()
    {
        var httpContextTenant1 = new Mock<IClusterChooserHttpContext>();
        httpContextTenant1.SetupGet(x => x.Headers).Returns(new Dictionary<string, StringValues>()
        {
            { "TenantId", new StringValues("1") }
        });
        var currentDateTime = new Mock<ICurrentDateTime>();
        currentDateTime.Setup(x => x.GetDateTime())
            .Returns(new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 2)));

        Add(new TestCaseInput(new List<RollingUpgradesRule>()
        {
            new("api", "cluster1",
                new HeaderRule("TenantId", "1", new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 1)), null)),
        }, httpContextTenant1.Object, currentDateTime.Object), "cluster1");
        Add(new TestCaseInput(new List<RollingUpgradesRule>()
        {
            new("api", "cluster1",
                new HeaderRule("TenantId", "1", new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 2)), null)),
        }, httpContextTenant1.Object, currentDateTime.Object), "cluster1");
        Add(new TestCaseInput(new List<RollingUpgradesRule>()
        {
            new("api", "cluster1",
                new HeaderRule("TenantId", "1", new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 3)), null)),
        }, httpContextTenant1.Object, currentDateTime.Object), null);
        Add(new TestCaseInput(new List<RollingUpgradesRule>()
        {
            new("api", "cluster1",
                new HeaderRule("TenantId", "1", new DateTimeOffset(new DateTime(2022, 1, 1, 1, 1, 1)), new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 2)))),
        }, httpContextTenant1.Object, currentDateTime.Object), "cluster1");
        Add(new TestCaseInput(new List<RollingUpgradesRule>()
        {
            new("api", "cluster1",
                new HeaderRule("TenantId", "1", new DateTimeOffset(new DateTime(2022, 1, 1, 1, 1, 2)), new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 3)))),
        }, httpContextTenant1.Object, currentDateTime.Object), "cluster1");
        Add(new TestCaseInput(new List<RollingUpgradesRule>()
        {
            new("api", "cluster1",
                new HeaderRule("TenantId", "1", new DateTimeOffset(new DateTime(2022, 1, 1, 1, 1, 3)), new DateTimeOffset(new DateTime(2023, 1, 1, 1, 1, 1)))),
        }, httpContextTenant1.Object, currentDateTime.Object), null);
    }
}