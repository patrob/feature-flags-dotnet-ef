using System.Net.Http.Json;
using FeatureFlagsEfDemo.Features.FeatureFlags;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Categories;

namespace FeatureFlagsEfDemo.Tests.Features.FeatureFlags;

public class FeatureFlagsIntegrationTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    [IntegrationTest]
    public async Task Get_ShouldListAllFeatures()
    {
        using var context = GetScopedContext();
        var expectedFeatureDtos = context.Features
            .Select(x => new FeatureDto
            {
                FeatureName = x.Name,
                IsEnabled = x.IsEnabled
            });
        var response = await Client.GetAsync("/Features");
        response.EnsureSuccessStatusCode();
        var features = await response.Content.ReadFromJsonAsync<FeatureDto[]>();
        features!.Should().BeEquivalentTo(expectedFeatureDtos);
    }
    
    [Fact]
    [IntegrationTest]
    public async Task SetIsEnabled_ShouldUpdateFeatureFlag()
    {
        var testFeature = FeatureEnum.Subtract.ToString();
        

        var payload = new FeatureDto { FeatureName = testFeature, IsEnabled = true };
        var response = await Client.PostAsJsonAsync($"/Features", payload);
        response.EnsureSuccessStatusCode();
        
        using var context = GetScopedContext();
        var expectedFeature = context.Features
            .AsNoTracking()
            .Single(x => x.Name == testFeature);
        expectedFeature.IsEnabled.Should().Be(payload.IsEnabled);
    }

    [Fact]
    [IntegrationTest]
    public async Task IsEnabledAsync_ShouldReturnFromDb()
    {
        var testFeature = FeatureEnum.Subtract.ToString();
        using var context = GetScopedContext();
        var feature = context.Features
            .Single(x => x.Name == testFeature);
        var handler = new FeatureHandler(context);
        var result = await handler.IsEnabledAsync<object?>(testFeature, null);
        result.Should().Be(feature.IsEnabled);
    }
}