using RabbitMQ.Client;

namespace ModulbankInternship.Infrastructure.Rabbit.Outbox.publisher;

public interface IMessagePublisher
{
    void Publish(string exchange, string routingKey, IBasicProperties props, byte[] body);
}