using System.Net;
using System.Net.Http.Json;
using FeatureFlagsEfDemo.Features.FeatureFlags;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit.Categories;

namespace FeatureFlagsEfDemo.Tests.Features.Calculator;

public class CalculatorIntegrationTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
{
    [Theory]
    [IntegrationTest]
    [InlineData("/health", null, HttpStatusCode.OK, null)]
    [InlineData("/Calculator/Add", new[] {1,2,3}, HttpStatusCode.OK, 6)]
    [InlineData("/Calculator/Multiply", new[] {2,5}, HttpStatusCode.OK, 10)]
    [InlineData("/Calculator/Divide", new[] {10, 2}, HttpStatusCode.OK, 5)]
    public async Task Get_ShouldReturnExpectedResults<T>(string endpoint,
                                                      int[]? payload,
                                                      HttpStatusCode expectedStatus,
                                                      T? expectedResult)
    {
        var queryParams = CreateNumberArrayQueryParameters("numbers", payload ?? []);
        var url = $"{endpoint}{queryParams}";
        var response = await Client.GetAsync(url);
        response.StatusCode.Should().Be(expectedStatus);
        if (expectedResult is not null && response.StatusCode == HttpStatusCode.OK)
            await VerifySuccessfulResult(expectedResult, response);
    }

    private static async Task VerifySuccessfulResult<T>(T? expectedResult, HttpResponseMessage response)
    {
        
            var responseContent = await response.Content.ReadFromJsonAsync<T>();
            responseContent.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task Subtract_Flag_Subtract_IsOn_ShouldReturnExpectedResult()
    {
        using var context = GetScopedContext();
        var feature = context.Features
            .Include(f => f.FeatureDetail)
            .Single(x => x.Name == FeatureEnum.Subtract.ToString());
        feature.FeatureDetail.IsEnabled = true;
        await context.SaveChangesAsync();

        const string url = "/Calculator/Subtract?numbers=10&numbers=5";
        var response = await Client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        await VerifySuccessfulResult(5, response);
    }
    
    [Fact]
    public async Task Subtract_Flag_Subtract_IsOff_ShouldReturnNotFound()
    {
        using var context = GetScopedContext();
        var feature = context.Features
            .Include(f => f.FeatureDetail)
            .Single(x => x.Name == FeatureEnum.Subtract.ToString());
        feature.FeatureDetail.IsEnabled = false;
        await context.SaveChangesAsync();
        
        const string url = "/Calculator/Subtract?numbers=10&numbers=5";
        var response = await Client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}