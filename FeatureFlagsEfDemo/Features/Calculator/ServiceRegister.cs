namespace FeatureFlagsEfDemo.Features.Calculator;

public static class ServiceRegister
{
    public static IServiceCollection AddCalculatorServices(this IServiceCollection services)
    {
        services.AddTransient<ICalculatorService, CalculatorService>();
        return services;
    }
}