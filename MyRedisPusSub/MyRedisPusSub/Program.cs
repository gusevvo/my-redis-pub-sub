using System.Text.Json;
using MyRedisPusSub;
using StackExchange.Redis;

using var redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
var channel = redis.GetSubscriber();

channel.Subscribe("sinus", HandleMessage);

Console.ReadKey();

void HandleMessage(RedisChannel redisChannel, RedisValue redisValue)
{
    var sinus = JsonSerializer.Deserialize<Sinus>(redisValue);
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {sinus}");

}

namespace MyRedisPusSub
{
    internal record Sinus(int Alpha, double Value);
}
