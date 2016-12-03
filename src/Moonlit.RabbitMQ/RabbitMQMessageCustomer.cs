using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Moonlit.RabbitMQ
{
    public class RabbitMQMessageCustomer<TMessage> : IMessageCustomer<TMessage>
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _queueName;
        private IModel _channel;
        private IConnection _connection;
        public RabbitMQMessageCustomer(ConnectionFactory connectionFactory, string queueName)
        {
            _logger = LogManager.GetLogger($"queue {queueName}");
            _connectionFactory = connectionFactory;
            _queueName = queueName;

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (s, e) =>
            {
                try
                {
                    _logger.Info($"message queue {_queueName} received a message");
                    var message = Encoding.UTF8.GetString(e.Body);
                    var data = JsonConvert.DeserializeObject<TMessage>(message);
                    Received(s, new EventArgs<TMessage>(data));
                    _channel.BasicAck(e.DeliveryTag, false);
                }
                catch (ApplicationException ex)
                {
                    // business we dont process
                    _logger.Error("fail to process message", ex);
                    _channel.BasicAck(e.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.Error("fail to process message:" + ex);
                    throw;
                }
            };
            _channel.BasicConsume(queue: _queueName, noAck: false, consumer: consumer);
        }

        private readonly ILog _logger;


        public event EventHandler<EventArgs<TMessage>> Received = delegate { };
    }
}