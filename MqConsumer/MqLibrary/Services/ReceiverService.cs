using MqLibrary.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MqLibrary.Services
{
    public class ReceiverService : IReceiverService
    {
        private readonly EventingBasicConsumer _consumer;
        private readonly IModel _channel;
        private readonly string _queue;

        public ReceiverService(string url, string queue, Action<MqUserObject> SubscribeMedthod)
        {
            _queue = queue;
            var factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                HostName = url
            };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue, exclusive: false);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, eventArgs) =>
            {
                var user = JsonConvert.DeserializeObject<MqUserObject>(Encoding.UTF8.GetString(eventArgs.Body.ToArray()));
                SubscribeMedthod(user);
            };
        }

        public void Start()
        {
            _channel.BasicConsume(queue: _queue, autoAck: true, consumer: _consumer);
            while (true) { Thread.Sleep(1000); }
            _channel.Close();

        }
    }
}
