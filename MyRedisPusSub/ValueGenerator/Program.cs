using System.Text.Json;
using StackExchange.Redis;
using static System.Math;


using var redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");

var channel = redis.GetSubscriber();

while (true)
{
    var alpha = DateTime.Now.Second * 6;
    var sinus = new Sinus(alpha, Sin(alpha));
    var value = JsonSerializer.Serialize(sinus);

    Console.WriteLine($"Publish {value}.");

    await channel.PublishAsync("sinus", value);

    await Task.Delay(1000);
}

internal record Sinus(int Alpha, double Value);

