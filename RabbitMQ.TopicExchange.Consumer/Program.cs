using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Consumer uygulaması ayakta !");

// Bağlantı Oluştur
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");
await using IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

// Topic Exchange Oluştur
await channel.ExchangeDeclareAsync(exchange: "topic-exchange-example", type: ExchangeType.Topic);

Console.WriteLine("Dinlenicek Topic Exchange'i belirtiniz: ");
var topicName = Console.ReadLine();

var queue = await channel.QueueDeclareAsync();
await channel.QueueBindAsync(queue: queue.QueueName, exchange: "topic-exchange-example", routingKey: topicName!);

AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicConsumeAsync(queue: queue.QueueName, autoAck: true, consumer: consumer);

consumer.ReceivedAsync += async (sender, e) =>
{
    var body = e.Body.ToArray();
    Console.WriteLine(Encoding.UTF8.GetString(body));
};

Console.ReadLine();