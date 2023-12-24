using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;

namespace BackgroundReceiver.RabbitMQ
{
    public class RabbitMqListener : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private ILogger _logger;

        private string _queue;

        public RabbitMqListener(IOptions<RabbitMqOptions> options, ILogger<RabbitMqListener> logger)
        {
            _queue = options.Value.Queue;

            var factory = new ConnectionFactory { HostName = options.Value.Host };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, eventArgs) =>
            {
                var content = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                // Каким-то образом обрабатываем полученное сообщение
                _logger.LogInformation($"({DateTime.Now}) Message received: {content}");

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(_queue, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
