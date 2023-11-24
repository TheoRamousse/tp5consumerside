using Microsoft.Extensions.Configuration;
using MqLibrary.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqLibrary.Services
{
    public class ProducerService : IProducerService
    {
        private IModel _channel;
        public ProducerService(IConfiguration _configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMqUrl"]
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.QueueDeclare(_configuration["RabbitMqQueue"], exclusive: false);
        }

        public void Close()
        {
            _channel.Close();
        }

        public void SendMessage(MqUserObject user)
        {
            var json = JsonConvert.SerializeObject(user);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(exchange: "", routingKey: "orders", body: body);
        }
    }
}
