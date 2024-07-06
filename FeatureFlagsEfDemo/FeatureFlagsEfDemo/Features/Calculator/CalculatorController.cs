using FeatureFlagsEfDemo.Features.FeatureFlags;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureFlagsEfDemo.Features.Calculator;

public class CalculatorController(ICalculatorService calculatorService) : ApiControllerBase
{
    [HttpGet]
    [Route("Add")]
    public int Add([FromQuery] int[] numbers)
    {
        return calculatorService.Add(numbers);
    }

    [HttpGet]
    [Route("Multiply")]
    public int Multiply([FromQuery] int[] numbers)
    {
        return calculatorService.Multiply(numbers);
    }

    [HttpGet]
    [Route("Subtract")]
    [FeatureGate(FeatureEnum.Subtract)]
    public int Subtract([FromQuery] int[] numbers)
    {
        return calculatorService.Subtract(numbers);
    }
    
    [HttpGet]
    [Route("Divide")]
    public int Divide([FromQuery] int[] numbers)
    {
        return calculatorService.Divide(numbers);
    }
}