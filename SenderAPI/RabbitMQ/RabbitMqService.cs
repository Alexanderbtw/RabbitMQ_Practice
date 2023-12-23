using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace SenderAPI.RabbitMQ
{
    public class RabbitMqService : IRabbitMqService
    {
        ConnectionFactory _factory { get; set; }
        string _queue { get; set; }

        public RabbitMqService(string host, string queue)
        {
            _factory = new ConnectionFactory() { HostName = host, Port = 5672 };
            _queue = queue;
        }

        public void SendMessage(string message)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: String.Empty,
                body: body,
                basicProperties: null,
                routingKey: _queue);
        }

        public void SendMessage(object obj)
        {
            var message = JsonSerializer.Serialize(obj);
            SendMessage(message);
        }
    }
}
