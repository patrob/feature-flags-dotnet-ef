using FeatureFlagsEfDemo.Features.Calculator;
using FeatureFlagsEfDemo.Features.FeatureFlags;
using FluentAssertions;
using NSubstitute;

namespace FeatureFlagsEfDemo.Tests.Features.Calculator;

public class CalculatorServiceTests
{
    private readonly IFeatureHandler _featureHandlerMock = Substitute.For<IFeatureHandler>();
    private readonly CalculatorService _sut;

    public CalculatorServiceTests()
    {
        _sut = new CalculatorService(_featureHandlerMock);
    }
    
    [Fact]
    public void Perform_WithEmptyInput_ShouldReturnZero()
    {
        var result = _sut.Add();
        result.Should().Be(0);
    }

    [Theory]
    [InlineData(new [] {1}, 1)]
    [InlineData(new [] {1,2}, 3)]
    [InlineData(new [] {1,2,3}, 6)]
    public void Add_ShouldAddAnyRangeOfNumbers(int[] numbers, int expected)
    {
        _sut.Add(numbers).Should().Be(expected);
    }
    
    [Theory]
    [InlineData(new [] {1}, 1)]
    [InlineData(new [] {1,2}, 2)]
    [InlineData(new [] {1,2,3}, 6)]
    [InlineData(new [] {1,2,3,4}, 24)]
    public void Multiply_ShouldMultiplyAnyRangeOfNumbers(int[] numbers, int expected)
    {
        _sut.Multiply(numbers).Should().Be(expected);
    }
    
    [Theory]
    [InlineData(new [] {1}, 1)]
    [InlineData(new [] {10,2}, 8)]
    [InlineData(new [] {12,4,2}, 6)]
    [InlineData(new [] {10,5,3}, 2)]
    public void Subtract_ShouldSubtractAnyRangeOfNumbers(int[] numbers, int expected)
    {
        _featureHandlerMock.IsEnabled(FeatureEnum.Subtract).Returns(true);
        _sut.Subtract(numbers).Should().Be(expected);
    }

    [Fact]
    public void Subtract_ShouldThrowExceptionIfNotEnabled()
    {
        _featureHandlerMock.IsEnabled(FeatureEnum.Subtract).Returns(false);
        var action = () => _sut.Subtract([1, 2, 3]);
        action.Should().Throw<NotImplementedException>();
    }
    
    [Theory]
    [InlineData(new [] {1}, 1)]
    [InlineData(new [] {10,2}, 5)]
    [InlineData(new [] {12,4,3}, 1)]
    [InlineData(new [] {100,10,5}, 2)]
    public void Divide_ShouldDivideAnyRangeOfNumbers(int[] numbers, int expected)
    {
        _sut.Divide(numbers).Should().Be(expected);
    }

    [Fact]
    public void Divide_Flag_HandleDivideByZeroRequests_IsOn_ShouldThrowInvalidDataException()
    {
        _featureHandlerMock.IsEnabled(FeatureEnum.HandleDivideByZeroRequests).Returns(true);
        var action = () => _sut.Divide([10, 0]);
        action.Should().Throw<InvalidDataException>()
            .WithMessage("Zero is not allowed when dividing");
    }

    [Fact]
    public void Divide_Flag_HandleDivideByZeroRequests_IsOff_ShouldThrowNumberException()
    {
        _featureHandlerMock.IsEnabled(FeatureEnum.HandleDivideByZeroRequests).Returns(false);
        var action = () => _sut.Divide([10, 0]);
        action.Should().Throw<DivideByZeroException>();
    }
}