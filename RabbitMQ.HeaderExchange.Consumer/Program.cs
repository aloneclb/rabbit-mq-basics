using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Consumer uygulaması ayakta !");

// Bağlantı Oluştur
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");
await using IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

// Header Exchange Oluştur
await channel.ExchangeDeclareAsync(exchange: "header-exchange-example", type: ExchangeType.Headers);

Console.WriteLine("Lütfen header value girin: ");
string? value = Console.ReadLine();

ArgumentNullException.ThrowIfNull(value);

var queue = await channel.QueueDeclareAsync();

await channel.QueueBindAsync(
    queue: queue.QueueName,
    exchange: "header-exchange-example",
    string.Empty,
    new Dictionary<string, object?>
    {
        { "no", value }
    });

AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicConsumeAsync(queue: queue.QueueName, autoAck: true, consumer: consumer);

consumer.ReceivedAsync += (sender, e) =>
{
    var body = e.Body.ToArray();
    Console.WriteLine(Encoding.UTF8.GetString(body));
    return Task.CompletedTask;
};

Console.ReadLine();