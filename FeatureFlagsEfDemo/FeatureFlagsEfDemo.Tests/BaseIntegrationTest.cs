using System.Text;
using FeatureFlagsEfDemo.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FeatureFlagsEfDemo.Tests;

[Collection("TestContainerDb")]
public class BaseIntegrationTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient Client = factory.CreateClient(new WebApplicationFactoryClientOptions
    {
        AllowAutoRedirect = true
    });

    protected CustomWebApplicationFactory Factory = factory;

    protected string CreateNumberArrayQueryParameters(string paramName, params int[] numbers)
    {
        if (numbers.Length == 0) return string.Empty;
        var sb = new StringBuilder();
        for (var i = 0; i < numbers.Length; i++)
        {
            if (i == 0) sb.Append($"?{paramName}={numbers[i]}");
            else sb.Append($"&{paramName}={numbers[i]}");
        }

        return sb.ToString();
    }

    protected IApplicationDbContext GetScopedContext()
    {
        return Factory.Services.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
    }
}