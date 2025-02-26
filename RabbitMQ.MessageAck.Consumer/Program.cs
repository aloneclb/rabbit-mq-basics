﻿// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Consumer uygulaması ayakta !");

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://hwwpdpny:btExE1dKV3tfRUrgIy1BAWxyJ4zcksCT@shark.rmq.cloudamqp.com/hwwpdpny");
await using IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();
await channel.QueueDeclareAsync(queue: "example-queue", exclusive: false);

// autoAck -> false olmalı, mesajın işlendiğini onaylamak için. Message Acknowledge özelliği.
AsyncEventingBasicConsumer consumer = new(channel);
await channel.BasicConsumeAsync(queue: "example-queue", autoAck: false, consumer: consumer);

consumer.ReceivedAsync += async (sender, e) =>
{
    //var body = e.Body.Span; Bu stack üzerindeki veri bundan dolayı asenkron işlemden önce kullanılamaz.
    var body = e.Body.ToArray();
    await Task.Delay(1000);
    Console.WriteLine(Encoding.UTF8.GetString(body));

    // Mesaj işlendiğinde, mesajın RabbitMQ'ya silinmesi gerektiğini bildir.
    // multiple parametresi -> false olur ise sadece bu mesajın işlendiğini bildirir.
    // multiple parametresi -> true olur ise bu mesajdan önceki tüm mesajların işlendiğini bildirir.
    await channel.BasicAckAsync(e.DeliveryTag, multiple: false);
};

// Eğer mesajın işlenemediği taktirde tekrar kuyruğa eklenmesini istersek requeue parametresini true yapmalıyız.
// await channel.BasicAckAsync(e.DeliveryTag, multiple: false, requeue: true);

Console.Read();