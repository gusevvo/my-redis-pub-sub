using Calculator.Engine.Models;
using Dapr.Actors;
using Dapr.Actors.Client;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Engine.Controllers;

[ApiController]
[Route("/calculation")]
public class CalculatorController : ControllerBase
{
    private readonly IActorProxyFactory _actorProxyFactory;
    private readonly ILogger<CalculatorController> _logger;

    public CalculatorController(
        IActorProxyFactory actorProxyFactory,
        ILogger<CalculatorController> logger)
    {
        _actorProxyFactory = actorProxyFactory;
        _logger = logger;
    }

    [HttpPost("{id:guid}/execute", Name = "ExecuteCalculation")]
    public async Task<IActionResult> ExecuteCalculation(
        [FromRoute] Guid id,
        [FromBody] ExecuteCalculationRequestModel request,
        CancellationToken cancellationToken)
    {
        var actorId = new ActorId(id.ToString());

        var actorProxy = request.Runtime switch
        {
            Runtime.Dotnet => _actorProxyFactory.Create(actorId, "DotnetCalculatorActor"),
            Runtime.Java => _actorProxyFactory.Create(actorId, "JavaCalculatorActor"),
            _ => throw new ArgumentOutOfRangeException()
        };

        var command = new ExecuteCalculationCommand(request.Expression, request.Parameters);
        var result = await actorProxy.InvokeMethodAsync<ExecuteCalculationCommand, object>("Execute", command, cancellationToken);

        return Ok(result);
    }

    [HttpPost("/value", Name = "PushValue")]
    public async Task<IActionResult> PushValue(
        [FromBody] ParameterValueModel message,
        CancellationToken cancellationToken)
    {
        await Task.Yield();

        _logger.LogInformation("Received: {message}", message);

        return Ok();
    }
}