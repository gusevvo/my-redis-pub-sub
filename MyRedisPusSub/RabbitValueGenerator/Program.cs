using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ValueGenerator;
using static System.Math;

const string exchangeName = "dapr-test";

var aliases = new[] { "A", "B", "C", "D" };//, "E", "F", "G", "H", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T" };
var random = new Random();


var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchangeName, "fanout");

while (true)
{
    var alpha = DateTime.Now.Second * 6;

    var parameter = new ParameterValue(
        aliases[random.Next(0, aliases.Length)],
        Sin(alpha).ToString("F2"),
        ParameterType.Double
    );

    var value = JsonSerializer.Serialize(parameter);
    Console.WriteLine($"Publish {value}.");

    channel.BasicPublish(
        // exchangeName,
        // alpha.ToString(),
        "",
        "dapr-test-values",
        null,
        Encoding.UTF8.GetBytes(value));

    await Task.Delay(1000);
}

namespace ValueGenerator
{
    public record ParameterValue(string Alias, string Value, ParameterType Type);
    public enum ParameterType
    {
        Double = 0,
        Long = 1,
        DateTime = 2,
        Boolean = 3,
        String = 4
    }
}
