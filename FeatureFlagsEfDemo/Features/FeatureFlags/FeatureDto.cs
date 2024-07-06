namespace FeatureFlagsEfDemo.Features.FeatureFlags;

public record FeatureDto
{
    public required string FeatureName { get; set; }
    public bool IsEnabled { get; set; }
}