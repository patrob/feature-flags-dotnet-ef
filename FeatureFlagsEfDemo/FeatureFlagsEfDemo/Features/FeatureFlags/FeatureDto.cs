namespace FeatureFlagsEfDemo.Features.FeatureFlags;

public record FeatureDto
{
    public string FeatureName { get; set; }
    public bool IsEnabled { get; set; }
}