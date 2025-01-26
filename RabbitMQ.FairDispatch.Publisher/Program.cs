using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Publisher !");

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");
await using IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();
await channel.QueueDeclareAsync(queue: "example-queue", exclusive: false);

// channel'a basicQos metodu ile fair display ayarı
// prefetchSize consumer tarafından alınabilecek en büyük mesaj boyutudur 0 sınırsız demek
// prefetchCount consumer tarafından aynı anda alınabilecek mesaj sayısı
// global false ise bu konfigurasyonun tüm consumerlar için mi yoksa sadece çağrı yapılan
// consumer için mi geçerli olduğunu belirler
await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

// mesajları gönder
byte[] message = Encoding.UTF8.GetBytes("Merhaba");
await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: message);

for (int i = 0; i < 100; i++)
{
    byte[] innerMessage = Encoding.UTF8.GetBytes($"Data {i}");
    await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: innerMessage);
}

Console.Read();