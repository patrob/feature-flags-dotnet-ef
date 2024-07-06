using FeatureFlagsEfDemo.Data;
using FeatureFlagsEfDemo.Features.FeatureFlags;
using FluentAssertions;
using NSubstitute;

namespace FeatureFlagsEfDemo.Tests.Features.FeatureFlags;

public class FeatureHandlerTests
{
    [Fact]
    public async Task GetFeatureNamesAsync_ShouldReturnEnumValuesAsStrings()
    {
        var featureNames = Enum.GetValues<FeatureEnum>()
            .Select(x => x.ToString())
            .ToList();
        var contextMock = Substitute.For<IApplicationDbContext>();
        var featureHandler = new FeatureHandler(contextMock);
        await foreach (var s in featureHandler.GetFeatureNamesAsync())
        {
            featureNames.Should().Contain(s);
        }
    }
}