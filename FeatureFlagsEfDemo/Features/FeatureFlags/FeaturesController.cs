using Microsoft.AspNetCore.Mvc;

namespace FeatureFlagsEfDemo.Features.FeatureFlags;

public class FeaturesController(IFeatureHandler featureHandler) : ApiControllerBase
{
    [HttpGet]
    public IEnumerable<FeatureDto> Get()
    {
        return featureHandler.GetFeatures();
    }

    [HttpPost]
    public void SetIsEnabled([FromBody] FeatureDto featureDto)
    {
        if (Enum.TryParse(featureDto.FeatureName, true, out FeatureEnum feature))
            featureHandler.SetIsEnabled(feature, featureDto.IsEnabled);
    }
}