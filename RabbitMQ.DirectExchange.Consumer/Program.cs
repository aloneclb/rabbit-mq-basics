using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Consumer uygulaması ayakta !");

// Bağlantı Oluştur
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");
await using IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

// 1- direct exchange'i publisher'daki ile birebir olucak şekilde channel'a tanımla
await channel.ExchangeDeclareAsync(exchange: "direct-exchange-example", type: ExchangeType.Direct);

// 2- Queue oluştur
var queue = await channel.QueueDeclareAsync();
var queueName = queue.QueueName;

// 3- Queue'yu exchange'e bind et (routing key ile)
await channel.QueueBindAsync(queue: queueName, exchange: "direct-exchange-example", routingKey: "direct-routing-key");

AsyncEventingBasicConsumer consumer = new(channel);
await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

consumer.ReceivedAsync += async (sender, e) =>
{
    var body = e.Body.ToArray();
    await Task.Delay(100);
    Console.WriteLine(Encoding.UTF8.GetString(body));
};

Console.Read();