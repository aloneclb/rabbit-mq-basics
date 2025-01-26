using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Publisher !");

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");
await using IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

// Topic Exchange
await channel.ExchangeDeclareAsync(exchange: "topic-exchange-example", type: ExchangeType.Topic);

// mesajları bu exchange'e bağlı tüm queue'lere gönder
for (int i = 0; i < 100; i++)
{
    await Task.Delay(100);
    byte[] byteMessage = Encoding.UTF8.GetBytes($"Mesaj {i}");

    Console.WriteLine("Mesajın gönderileceği topic formatını girin: ");
    var topicName = Console.ReadLine();
    if (string.IsNullOrEmpty(topicName))
    {
        continue;
    }

    await channel.BasicPublishAsync(exchange: "topic-exchange-example", routingKey: topicName, body: byteMessage);
}

Console.Read();