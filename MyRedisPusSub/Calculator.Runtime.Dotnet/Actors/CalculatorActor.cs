using Calculator.Runtime.Dotnet.Models;
using Calculator.Runtime.Dotnet.Services;
using Dapr.Actors.Runtime;

namespace Calculator.Runtime.Dotnet.Actors;

public class CalculatorActor : Actor, ICalculatorActor
{
    private readonly ICalculationService _calculationService;
    private readonly IParametersTransformationService _transformationService;

    public CalculatorActor(ActorHost host,
        ICalculationService calculationService,
        IParametersTransformationService transformationService) : base(host)
    {
        _calculationService = calculationService;
        _transformationService = transformationService;
    }

    public async Task<object> ExecuteAsync(Calculation calculation)
    {
        var parameters = _transformationService.Transform(calculation.Parameters);
        var command = new CalculationCommand(calculation.Expression, parameters);

        var result = await _calculationService.ExecuteAsync(command);

        return result;
    }

    public Task<int> TwiceAsync(int value)
    {
        return Task.FromResult(value * 2);
    }
}