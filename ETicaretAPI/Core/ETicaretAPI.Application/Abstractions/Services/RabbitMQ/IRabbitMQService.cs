using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services.RabbitMQ
{
    public interface IRabbitMQService
    {
        public void SendMessageToExchange(string exchangeName, string exchangeType, string queueName, object obj);
        public void Consume<T>(string exchangeName, string exchangeType, string queueName, Action<T> action);

    }
}
