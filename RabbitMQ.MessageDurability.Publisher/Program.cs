// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Publisher !");

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");
await using IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

// 3- Queue oluştur
// exclusive -> false olmalı, channela özel olmaması için.
// durable -> true olmalı, mesajların kalıcı olması için.
// eğer bir queue'nun publisher'da durable parametresi true verilmiş ise consumer'dada true olmalıdır.
await channel.QueueDeclareAsync(queue: "example-queue", exclusive: false, durable: true);

byte[] message = Encoding.UTF8.GetBytes("Merhaba");
await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: message);

var messageProperties = new BasicProperties()
{
    Persistent = true // Mesajın kalıcı olması için !!
};

for (int i = 0; i < 100; i++)
{
    byte[] innerMessage = Encoding.UTF8.GetBytes($"Data {i}");
    await channel.BasicPublishAsync(
        exchange: "",
        routingKey: "example-queue",
        mandatory: true, // Mesajın mutlaka bir kuyruğa ulaşmasını istiyorsanız kullanılır.
        basicProperties: messageProperties,
        body: innerMessage);
}

// mandatory: true kullanıldığında, RabbitMQ'da geri çağırma mekanizmasını (basic.return event)
// düzgün bir şekilde ele almanız gerekir. Bununla, mesajın neden bir kuyruğa ulaşmadığını
// öğrenebilirsiniz (örneğin, uygun bir routingKey eşleşmediği için).

// BasicReturn event'ini tanımlıyoruz
//channel.BasicReturnAsync += async (sender, args) =>
//{
//    // Mesajın geri döndüğünde ne yapılacağını belirtiyoruz
//    Console.WriteLine("Mesaj geri döndü:");
//    Console.WriteLine($"Exchange: {args.Exchange}");
//    Console.WriteLine($"Routing Key: {args.RoutingKey}");
//    Console.WriteLine($"Mesaj: {Encoding.UTF8.GetString(args.Body.ToArray())}");
//};

Console.Read();