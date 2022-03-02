using System.Numerics;
using Exercise01.Exceptions;
using Exercise01.Services;
using FluentAssertions;
using Xunit;

namespace Exercise01.Tests.Unit.Services;

public class TowardsTests
{
    [Theory]
    [InlineData(100, "one hundred")]
    [InlineData(2, "two")]
    [InlineData(13, "thirteen")]
    [InlineData(21, "twenty one")]
    [InlineData(101, "one hundred and one")]
    [InlineData(111, "one hundred and eleven")]
    [InlineData(1_001, "one thousand, and one")]
    [InlineData(1_000_001, "one million, and one")]
    [InlineData(2_003_223_999_001, "two billion, three million, two hundred, and twenty three thousand, nine hundred and ninety nine million, and one")]
    public void Towards_ShouldReturnCorrectWordForNumber_IfIntProvidedIsValid(int number, string expectedResult)
    {
        // Arrange
        var sut = TowardsService.Towards<int>;

        // Act
        var result = sut(number);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("18_456_002_032_001_000_007",
        "eighteen quintillion four hundred and fifty-six quadrillion, two trillion, thirty-two billion, one million, and seven")]
    public void Towards_ShouldReturnCorrectWordForNumber_IfBigIntergerProvidedIsValid(string numStr, string expectedResult)
    {
        // Arrange
        var number = BigInteger.Parse(numStr);
        
        var sut = TowardsService.Towards<BigInteger>;

        // Act
        var result = sut(number);

        // Assert
        result.Should().Be(expectedResult);
    }
    
    [Fact]
    public void Towards_ShouldNotThrowAnyExceptions_IfIntProvidedIsValid()
    {
        // Arrange
        var sut = TowardsService.Towards<int>;

        // Act
        var result = () => sut(1);

        // Assert
        result.Should().NotThrow();
    }
    
    [Fact]
    public void Towards_ShouldNotThrowAnyExceptions_IfBigIntegerProvidedIsValid()
    {
        // Arrange
        var sut = TowardsService.Towards<BigInteger>;

        // Act
        var result = () => sut(BigInteger.Parse("1"));

        // Assert
        result.Should().NotThrow();
    }

    [Fact]
    public void Towards_ShouldThrowInvalidTypeException_IfTypeProvidedIsNotIntOrBigInterger()
    {
        // Arrange
        var sut = TowardsService.Towards<double>;
        
        // Act
        var result = () => sut(1D);
        
        // Assert
        result.Should()
            .ThrowExactly<InvalidTypeException>()
            .WithMessage("Expected type of Int32 or BigInteger but got Double");
    }
}