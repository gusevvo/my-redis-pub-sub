using System.Dynamic;
using System.Reflection;
using Calculator.Runtime.Dotnet.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Calculator.Runtime.Dotnet.Services;

public class CalculationService : ICalculationService
{
    public async Task<object> ExecuteAsync(CalculationCommand calculation)
    {
        var globals = new Globals(ToAnonymousObject(calculation.Parameters));

        var refs = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.DynamicAttribute).GetTypeInfo().Assembly.Location)
        };

        var result = await CSharpScript.EvaluateAsync<object>(calculation.Expression, ScriptOptions.Default.AddReferences(refs), globals);

        return result;
    }

    private dynamic ToAnonymousObject(IDictionary<string, object> parameters)
    {
        var result = new ExpandoObject() as IDictionary<string, object>;

        foreach (var parameter in parameters)
        {
            result[parameter.Key] = parameter.Value;
        }

        return result;

        // return new DynamicDictionary(parameters);
    }

    public record Globals(dynamic Param);


    // public class DynamicDictionary : DynamicObject
    // {
    //     private readonly IDictionary<string, object?> _dictionary;
    //
    //     public DynamicDictionary(IDictionary<string, object?> dictionary)
    //     {
    //         _dictionary = dictionary;
    //     }
    //
    //     public override bool TryGetMember(GetMemberBinder binder, out object? result)
    //     {
    //         return _dictionary.TryGetValue(binder.Name, out result);
    //     }
    //
    //     public override bool TrySetMember(SetMemberBinder binder, object? value)
    //     {
    //         _dictionary[binder.Name] = value;
    //         return true;
    //     }
    // }
}