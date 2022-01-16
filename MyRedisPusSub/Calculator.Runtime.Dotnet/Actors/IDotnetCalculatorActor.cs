using Calculator.Runtime.Dotnet.Models;
using Dapr.Actors;

namespace Calculator.Runtime.Dotnet.Actors;

public interface IDotnetCalculatorActor : IActor
{
    Task<object> Execute(Calculation calculation);
}