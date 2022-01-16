using Calculator.Runtime.Dotnet.Actors;
using Calculator.Runtime.Dotnet.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container here

builder.Services.AddTransient<ICalculationService, CalculationService>();
builder.Services.AddTransient<IParametersTransformationService, ParametersTransformationService>();
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<CalculatorActor>();
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.MapActorsHandlers();

app.Run();