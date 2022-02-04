using AnalyticsMicroservice.API.Entities;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace AnalyticsMicroservice.API.Rabbit
{
    public class Publisher
    {
        private readonly ConnectionFactory factory;

        public Publisher()
        {
            this.factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
        }

        public void Publish(DataAnalytics content, string queueName)
        {
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string datajson = JsonConvert.SerializeObject(content);
                var body = Encoding.UTF8.GetBytes(datajson);
                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     mandatory: true,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
