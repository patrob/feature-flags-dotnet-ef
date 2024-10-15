using FeatureFlagsEfDemo.Data;
using Microsoft.FeatureManagement;

namespace FeatureFlagsEfDemo.Features.FeatureFlags;

public interface IFeatureHandler : IFeatureManager
{
    IEnumerable<FeatureDto> GetFeatures();
    bool IsEnabled(FeatureEnum feature);
    void SetIsEnabled(FeatureEnum feature, bool isEnabled);
}

public class FeatureHandler(IApplicationDbContext context) : IFeatureHandler
{
    public async IAsyncEnumerable<string> GetFeatureNamesAsync()
    {
        foreach (var feature in Enum.GetValues<FeatureEnum>())
        {
            yield return await Task.FromResult(feature.ToString());
        }
    }

    public bool IsEnabled(FeatureEnum feature)
    {
        var featureModel = context.Features
            .Single(x => x.Name == feature.ToString());
        return featureModel.IsEnabled;
    }

    public void SetIsEnabled(FeatureEnum feature, bool isEnabled)
    {
        var featureModel = context.Features
            .Single(x => x.Name == feature.ToString());
        featureModel.IsEnabled = isEnabled;
        context.SaveChanges();
    }

    public async Task<bool> IsEnabledAsync(string feature)
    {
        var isDefinedFeature = Enum.TryParse(feature, true, out FeatureEnum featureEnum);
        return await Task.FromResult(isDefinedFeature && IsEnabled(featureEnum));
    }

    public async Task<bool> IsEnabledAsync<TContext>(string feature, TContext contextObject)
    {
        return await IsEnabledAsync(feature);
    }

    public IEnumerable<FeatureDto> GetFeatures()
    {
        return context.Features
            .Select(x => new FeatureDto
            {
                FeatureName = x.Name,
                IsEnabled = x.IsEnabled
            });
    }
}