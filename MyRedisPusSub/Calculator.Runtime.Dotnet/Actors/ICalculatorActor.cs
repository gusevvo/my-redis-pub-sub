using Calculator.Runtime.Dotnet.Models;
using Dapr.Actors;

namespace Calculator.Runtime.Dotnet.Actors;

public interface ICalculatorActor : IActor
{
    Task<object> ExecuteAsync(Calculation calculation);

    Task<int> TwiceAsync(int value);
}