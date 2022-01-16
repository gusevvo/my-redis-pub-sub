using System.Collections.Generic;
using System.Threading.Tasks;
using Calculator.Runtime.Dotnet.Services;
using FluentAssertions;
using Xunit;

namespace Calculator.Runtime.Dotnet.Tests.Services;

public class CalculationServiceTests
{
    private readonly ICalculationService _sut = new CalculationService();

    [Fact]
    public async Task ExecuteAsync_Should_Return_Valid_Value()
    {
        // arrange
        var calculation = new CalculationCommand("Param.A + Param.B", new Dictionary<string, object>
        {
            ["A"] = 1,
            ["B"] = 2
        });
        var expected = (object)3;

        // act
        var actual = await _sut.ExecuteAsync(calculation);

        // assert
        actual.Should().BeEquivalentTo(expected);
    }
}