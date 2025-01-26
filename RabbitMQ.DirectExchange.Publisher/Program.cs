using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Publisher !");

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");
await using IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

// Direct Exchange
// Mesajların direkt olarak belirli bir kuyruğa gönderilmesini sağlayan exchange tipidir.
// Her bir mesajın bir routing key'i vardır ve bu routing key'e göre mesajlar ilgili kuyruğa yönlendirilir.
await channel.ExchangeDeclareAsync(exchange: "direct-exchange-example", type: ExchangeType.Direct);

// mesajları gönder
while (true)
{
    Console.WriteLine("Mesaj giriniz: ");
    string? message = Console.ReadLine();
    if (string.IsNullOrEmpty(message))
    {
        Console.WriteLine("Mesaj boş olamaz !");
        continue;
    }

    // mesajı byte array'e çevir.
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

    // routing key belirtilerek mesajın hangi kuyruğa gönderileceği belirlenir
    await channel.BasicPublishAsync(exchange: "direct-exchange-example", routingKey: "direct-routing-key", body: byteMessage);
}
