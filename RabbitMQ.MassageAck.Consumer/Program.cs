//// See https://aka.ms/new-console-template for more information
//using RabbitMQ.Client;
//using System.Text;

Console.WriteLine("Publisher !");


//// 1- RabbitMQ'ya bağlantı oluştur
//ConnectionFactory factory = new ConnectionFactory();
//factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");

//// 2- Bağlantıyı aktifleştir ve channel aç
//await using IConnection connection = await factory.CreateConnectionAsync();
//IChannel channel = await connection.CreateChannelAsync();


//// 3- Queue oluştur
//// exclusive -> false olmalı, channela özel olmaması için.
//await channel.QueueDeclareAsync(queue: "example-queue", exclusive: false);


//// 4- Queue'ya mesaj gönder
//// RabbitMQ kuyruğa atılan mesajları byte türünden kabul eder. Haliyle göndericeğimiz mesajı byte türüne 
//// dönüştürmemiz gerekli.
//byte[] message = Encoding.UTF8.GetBytes("Merhaba");
//await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: message);


//for (int i = 0; i < 100; i++)
//{
//    byte[] innerMessage = Encoding.UTF8.GetBytes($"Data {i}");
//    await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: innerMessage);
//}

//Console.Read();