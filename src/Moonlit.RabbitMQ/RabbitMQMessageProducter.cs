using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Moonlit.RabbitMQ
{
    public class RabbitMQMessageProducter<TMessage> : IMessageProducter<TMessage>
    {
        private readonly string _queueName;
        private ConnectionFactory _factory;
        private IModel _channel;
        private IConnection _connection;
        public RabbitMQMessageProducter(ConnectionFactory connectionFactory, string queueName)
        {
            _queueName = queueName;
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            _channel.BasicQos(0, 1, false);
        }

        private ILog logger = LogManager.GetLogger(typeof(RabbitMQMessageCustomer<TMessage>));


        public Task PostAsync(TMessage message)
        {
            logger.Debug($"post a message into queue {_queueName}");
            var text = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(text);
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            _channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: properties,
                                 body: body);
            return Task.FromResult(0);
        }
    }
}
