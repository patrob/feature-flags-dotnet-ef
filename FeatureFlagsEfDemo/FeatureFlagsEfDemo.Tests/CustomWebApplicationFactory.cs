using FeatureFlagsEfDemo.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FeatureFlagsEfDemo.Tests;

[Collection("TestContainerDb")]
// ReSharper disable once ClassNeverInstantiated.Global
public class CustomWebApplicationFactory(DatabaseFixture databaseFixture) : WebApplicationFactory<Program>
{
    public async Task ResetDatabase()
    {
        await databaseFixture.ResetAsync();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((_, options) =>
                {
                    options.UseSqlServer(databaseFixture.GetConnection()!);
                });
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
        });
    }
}