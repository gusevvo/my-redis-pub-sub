namespace Calculator.Engine.Models;

public record ExecuteCalculationRequestModel(
    string Expression,
    Runtime Runtime,
    ICollection<ParameterModel> Parameters);

public record ExecuteCalculationCommand(
    string Expression,
    ICollection<ParameterModel> Parameters);

public record ParameterModel(string Alias, string Value, ParameterType Type);

public enum ParameterType
{
    Double = 0,
    Long = 1,
    DateTime = 2,
    Boolean = 3,
    String = 4
}