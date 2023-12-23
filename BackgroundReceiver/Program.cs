using BackgroundReceiver.RabbitMQ;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BackgroundReceiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddMqListener(builder.Configuration.GetSection("RabbitMQ"));

            var host = builder.Build();
            host.Run();
        }
    }
}