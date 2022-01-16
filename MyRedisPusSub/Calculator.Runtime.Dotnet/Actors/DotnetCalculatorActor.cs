using Calculator.Runtime.Dotnet.Models;
using Calculator.Runtime.Dotnet.Services;
using Dapr.Actors.Runtime;

namespace Calculator.Runtime.Dotnet.Actors;

public class DotnetCalculatorActor : Actor, IDotnetCalculatorActor
{
    private readonly ICalculationService _calculationService;
    private readonly IParametersTransformationService _transformationService;
    private readonly ILogger<DotnetCalculatorActor> _logger;

    public DotnetCalculatorActor(ActorHost host,
        ICalculationService calculationService,
        IParametersTransformationService transformationService,
        ILogger<DotnetCalculatorActor> logger) : base(host)
    {
        _calculationService = calculationService;
        _transformationService = transformationService;
        _logger = logger;
    }

    public async Task<object> Execute(Calculation calculation)
    {
        _logger.LogInformation("Received: {calculation}", calculation);
        _logger.LogInformation("Received: {parameters}", calculation.Parameters);

        var parameters = _transformationService.Transform(calculation.Parameters);
        var command = new CalculationCommand(calculation.Expression, parameters);

        var result = await _calculationService.ExecuteAsync(command);

        return result;
    }
}