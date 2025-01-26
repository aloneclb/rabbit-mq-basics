using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Consumer uygulaması ayakta !");

// Bağlantı Oluştur
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");
await using IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

// Fanout Exchange Oluştur
await channel.ExchangeDeclareAsync(exchange: "fanout-exchange-example", type: ExchangeType.Fanout);

Console.WriteLine("Fanout Exchange adını giriniz: ");
var queueName = Console.ReadLine();

var queue = await channel.QueueDeclareAsync(queue: queueName ?? string.Empty, exclusive: false);

await channel.QueueBindAsync(queue: queueName, exchange: "fanout-exchange-example", routingKey: string.Empty);

// consumer oluştur
AsyncEventingBasicConsumer consumer = new(channel);
await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

consumer.ReceivedAsync += async (sender, e) =>
{
    var body = e.Body.ToArray();
    await Task.Delay(100);
    Console.WriteLine(Encoding.UTF8.GetString(body));
};

Console.ReadLine();