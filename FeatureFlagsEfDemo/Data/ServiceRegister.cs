using Microsoft.EntityFrameworkCore;

namespace FeatureFlagsEfDemo.Data;

public static class ServiceRegister
{
    public static IServiceCollection AddApplicationDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(builder =>
            builder.UseSqlServer(config.GetConnectionString("ApplicationDb")));
        services.AddTransient<IApplicationDbContext>(s => s.GetRequiredService<ApplicationDbContext>());
        return services;
    }
}