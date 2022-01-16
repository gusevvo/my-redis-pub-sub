namespace Calculator.Runtime.Dotnet.Services;

// ReSharper disable once ClassNeverInstantiated.Global
public record CalculationCommand(string Expression, IDictionary<string, object> Parameters);