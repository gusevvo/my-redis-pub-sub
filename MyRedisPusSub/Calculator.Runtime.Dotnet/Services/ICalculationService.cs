using Calculator.Runtime.Dotnet.Models;

namespace Calculator.Runtime.Dotnet.Services;

public interface ICalculationService
{
    Task<object> ExecuteAsync(CalculationCommand calculation);
}