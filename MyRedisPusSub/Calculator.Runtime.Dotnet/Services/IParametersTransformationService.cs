
using Calculator.Runtime.Dotnet.Models;

namespace Calculator.Runtime.Dotnet.Services;

public interface IParametersTransformationService
{
    public IDictionary<string, object> Transform(ICollection<Parameter> parameters);
}