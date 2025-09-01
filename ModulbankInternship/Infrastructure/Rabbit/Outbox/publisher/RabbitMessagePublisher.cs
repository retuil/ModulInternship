using RabbitMQ.Client;

namespace ModulbankInternship.Infrastructure.Rabbit.Outbox.publisher;

public class RabbitMessagePublisher(IModel channel) : IMessagePublisher
{
    public void Publish(string exchange, string routingKey, IBasicProperties props, byte[] body)
        => channel.BasicPublish(exchange, routingKey, props, body);
}