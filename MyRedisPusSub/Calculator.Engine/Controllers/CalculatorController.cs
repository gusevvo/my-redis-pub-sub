using Calculator.Engine.Models;
using Dapr.Actors;
using Dapr.Actors.Client;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Engine.Controllers;

[ApiController]
[Route("/calculator")]
public class CalculatorController : ControllerBase
{
    private readonly IActorProxyFactory _actorProxyFactory;

    public CalculatorController(IActorProxyFactory actorProxyFactory)
    {
        _actorProxyFactory = actorProxyFactory;
    }

    [HttpPost("execute", Name = "ExecuteCalculation")]
    public async Task<IActionResult> ExecuteCalculation(ExecuteCalculationRequestModel request,
        CancellationToken cancellationToken)
    {
        var guid = Guid.NewGuid().ToString();
        var actorId = new ActorId(guid);

        var actorProxy = request.Runtime switch
        {
            Runtime.Dotnet => _actorProxyFactory.Create(actorId, "DotnetCalculatorActor"),
            Runtime.Java => _actorProxyFactory.Create(actorId, "JavaCalculatorActor"),
            _ => throw new ArgumentOutOfRangeException()
        };

        var result = await actorProxy.InvokeMethodAsync<ExecuteCalculationRequestModel, object>("Execute", request, cancellationToken);

        return Ok(result);
    }
}