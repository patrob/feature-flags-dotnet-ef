using FeatureFlagsEfDemo.Features.FeatureFlags;

namespace FeatureFlagsEfDemo.Features.Calculator;

public interface ICalculatorService
{
    int Add(params int[] numbers);
    int Subtract(params int[] numbers);
    int Multiply(params int[] numbers);
    int Divide(params int[] numbers);
}

public class CalculatorService(IFeatureHandler featureHandler) : ICalculatorService
{
    public int Perform(Func<int, int, int> aggregate, params int[] numbers)
    {
        return numbers.Length == 0
            ? 0
            : numbers.Aggregate(aggregate);
    }

    public int Add(params int[] numbers)
    {
        return Perform((curr, next) => curr + next, numbers);
    }

    public int Subtract(params int[] numbers)
    {
        if (!featureHandler.IsEnabled(FeatureEnum.Subtract)) throw new NotImplementedException();
        return Perform((curr, next) => curr - next, numbers);
    }

    public int Multiply(params int[] numbers)
    {
        return Perform((curr, next) => curr * next, numbers);
    }

    public int Divide(params int[] numbers)
    {
        if (featureHandler.IsEnabled(FeatureEnum.HandleDivideByZeroRequests) && numbers.Any(x => x == 0))
            throw new InvalidDataException("Zero is not allowed when dividing");
        return Perform((curr, next) => curr / next, numbers);
    }
}