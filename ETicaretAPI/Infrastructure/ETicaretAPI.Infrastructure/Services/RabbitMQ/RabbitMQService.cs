using ETicaretAPI.Application.Abstractions.Services.RabbitMQ;
using ETicaretAPI.Infrastructure.Factories;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        readonly IConfiguration _configuration;
        // KONFIGURASYONLARIN TAM DEĞİL CONNECTION YAPARKENKI. UNUTMA!!!!
        // to do: Bir Worker oluştur. ResetPassword için auth servisi kaldırıp publish et. Workerdan ise publish edileni consume et.
        public RabbitMQService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Consume<T>(string exchangeName, string exchangeType, string queueName, Action<T> action)
        {
            var channel = QueueFactory.CreateBasicConsumer(_configuration["RabbitMQ:HostName"],
                                                            _configuration["RabbitMQ:UserName"],
                                                            _configuration["RabbitMQ:Password"],
                                                            _configuration["RabbitMQ:VirtualHost"],
                                                            int.Parse(_configuration["RabbitMQ:Port"]))
                   .EnsureExchange(exchangeName, exchangeType)
                   .EnsureQueue(queueName, exchangeName);


            channel.Received += (m,eventArgs) => {

                var body = eventArgs.Body.ToArray();

                var obj = Encoding.UTF8.GetString(body);
                var model = JsonSerializer.Deserialize<T>(obj);
                action(model);

                channel.Model.BasicAck(eventArgs.DeliveryTag,false);
                
            };

            channel.Model.BasicConsume(queueName, autoAck: false,consumer:channel);
        }

        public void SendMessageToExchange(string exchangeName, string exchangeType, string queueName, object obj)
        {
            var channel = QueueFactory.CreateBasicConsumer(_configuration["RabbitMQ:HostName"],
                                                            _configuration["RabbitMQ:UserName"],
                                                            _configuration["RabbitMQ:Password"],
                                                            _configuration["RabbitMQ:VirtualHost"],
                                                            int.Parse(_configuration["RabbitMQ:Port"]))
                    .EnsureExchange(exchangeName, exchangeType)
                    .EnsureQueue(queueName, exchangeName);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));

            channel.Model.BasicPublish(exchangeName, queueName, basicProperties: null, body: body);
        }
    }
}
