using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundReceiver.RabbitMQ
{
    public static class RabbitMqListenerExtensions
    {
        public static IServiceCollection AddMqListener(this IServiceCollection collection, IConfiguration config)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (config == null) throw new ArgumentNullException(nameof(config));

            collection.Configure<RabbitMqOptions>(config);
            return collection.AddHostedService<RabbitMqListener>();
        }
    }
}
