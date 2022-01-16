using Calculator.Runtime.Dotnet.Models;

namespace Calculator.Runtime.Dotnet.Services;

public class ParametersTransformationService : IParametersTransformationService
{
    public IDictionary<string, object> Transform(ICollection<Parameter> parameters)
    {
        return parameters.ToDictionary(
            k => k.Alias,
            v => v.Type switch
            {
                ParameterType.Double => double.Parse(v.Value),
                ParameterType.Long => long.Parse(v.Value),
                ParameterType.DateTime => DateTime.Parse(v.Value),
                ParameterType.Boolean => bool.Parse(v.Value),
                ParameterType.String => (object)v.Value,
                _ => throw new ArgumentOutOfRangeException()
            });
    }
}