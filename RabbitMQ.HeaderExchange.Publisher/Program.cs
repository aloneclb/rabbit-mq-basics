using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Publisher !");

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");
await using IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

// Header Exchange
await channel.ExchangeDeclareAsync(exchange: "header-exchange-example", type: ExchangeType.Headers);

// mesajları bu exchange'e bağlı tüm queue'lere gönder
for (int i = 0; i < 100; i++)
{
    await Task.Delay(100);
    byte[] byteMessage = Encoding.UTF8.GetBytes($"Mesaj {i}");

    Console.WriteLine("Mesajın gönderileceği header value girin: ");
    var value = Console.ReadLine();
    if (string.IsNullOrEmpty(value))
    {
        continue;
    }

    var properties = new BasicProperties()
    {
        Headers = new Dictionary<string, object?>
        {
            { "no", value }
        }
    };

    await channel.BasicPublishAsync(
        exchange: "header-exchange-example",
        routingKey: string.Empty,
        mandatory: true,
        body: byteMessage,
        basicProperties: properties);
}

Console.Read();