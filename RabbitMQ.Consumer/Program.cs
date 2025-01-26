using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Consumer uygulaması ayakta !");

// 1- Bağlantı Oluştur
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");

// 2- Bağlantıyı aktifleştir ve kanal aç
await using IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

// 3- Queue oluştur
// exclusive -> false olmalı, channela özel olmaması için.
await channel.QueueDeclareAsync(queue: "example-queue", exclusive: false);

// 4- Queue'dan mesaj oku
AsyncEventingBasicConsumer consumer = new(channel);
await channel.BasicConsumeAsync(queue: "example-queue", false, consumer: consumer);

consumer.ReceivedAsync += async (sender, e) =>
{
    //var body = e.Body.Span; Bu stack üzerindeki veri bundan dolayı asenkron işlemden önce kullanılamaz.
    var body = e.Body.ToArray();
    await Task.Delay(100);

    Console.WriteLine(Encoding.UTF8.GetString(body));
};

Console.Read();