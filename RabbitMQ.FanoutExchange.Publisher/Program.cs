using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Publisher !");

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");
await using IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

// Fanout Exchange
await channel.ExchangeDeclareAsync(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);

// mesajları bu exchange'e bağlı tüm queue'lere gönder
for (int i = 0; i < 100; i++)
{
    await Task.Delay(100);
    byte[] byteMessage = Encoding.UTF8.GetBytes($"Mesaj {i}");

    await channel.BasicPublishAsync(exchange: "fanout-exchange-example", routingKey: string.Empty, body: byteMessage);
}

Console.Read();
