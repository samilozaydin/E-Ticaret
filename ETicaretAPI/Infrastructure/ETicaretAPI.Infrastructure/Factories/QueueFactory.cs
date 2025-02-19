using ETicaretAPI.Application.Consts;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Factories
{
    public static class QueueFactory
    {
       /* public static void SendMessage(string exchangeName,string exchangeType,string queueName,object obj ) 
        {
            var channel = CreateBasicConsumer(RabbitMQConstants.RabbitMQHost)
                .EnsureExchange(exchangeName,exchangeType)
                .EnsureQueue(queueName,exchangeName);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));

            channel.Model.BasicPublish(exchangeName, queueName,basicProperties:null, body: body);
        }*/

        public static EventingBasicConsumer CreateBasicConsumer(string? hostName,string? userName,string? password, string? vhost, int? port)
        {
            var factory = new ConnectionFactory() { 
                HostName = hostName ?? default,
                UserName = userName ?? default,
                Password = password ?? default,
                VirtualHost = vhost ?? default,
                Port = port ?? default
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            return new EventingBasicConsumer(channel);
        }

        public static EventingBasicConsumer EnsureExchange(this EventingBasicConsumer consumer,
                                                            string exchangeName, 
                                                            string exchangeType = RabbitMQConstants.DefaultExchangeType) 
        {
            consumer.Model.ExchangeDeclare(exchangeName,exchangeType,durable:false,autoDelete:false);

            return consumer;
        }
        public static EventingBasicConsumer EnsureQueue(this EventingBasicConsumer consumer,
                                                    string queueName,
                                                    string exchangeName)        {
            consumer.Model.QueueDeclare(queueName,durable:false,autoDelete:false,exclusive:false);
            consumer.Model.QueueBind(queueName, exchangeName, queueName);

            return consumer;
        }

    }
}
