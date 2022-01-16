namespace Calculator.Runtime.Dotnet.Models;

// ReSharper disable once ClassNeverInstantiated.Global
public record Calculation(string Expression, ICollection<Parameter> Parameters);