using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;

namespace FeatureFlagsEfDemo.Features.FeatureFlags;

public static class ServiceRegister
{
    public static IServiceCollection AddFeatureFlags(this IServiceCollection services)
    {
        services.AddFeatureManagement(); // needed for some built-in stuff
        services.RemoveAll<IFeatureManager>()
            .AddTransient<IFeatureManager, FeatureHandler>(); // override with our stuff
        services.AddTransient<IFeatureHandler, FeatureHandler>();
        return services;
    }
}