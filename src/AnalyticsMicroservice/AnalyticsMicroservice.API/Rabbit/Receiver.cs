using AnalyticsMicroservice.API.Analyser;
using AnalyticsMicroservice.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AnalyticsMicroservice.API.Rabbit
{
    public class Receiver
    {
        private ConnectionFactory factory;
        private IModel channel;
        private IConnection connection;
        private DataAnalyser _da;

        public Receiver(IDistributedCache redis)
        {
            _da = new DataAnalyser(redis);
            factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void Subscribe(string qName)
        {
            //channel.ExchangeDeclare(exchange: qName, type: ExchangeType.Fanout);
            channel.QueueDeclare(queue: qName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            //var queueName = channel.QueueDeclare().QueueName;
            /*channel.QueueBind(queue: queueName,
                              exchange: qName,
                              routingKey: "");*/
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Analyst service: Recieved [JSON message] from DataService: message=" + message);
                Data sensorData = JsonConvert.DeserializeObject<Data>(message.ToString());

                _da.AnalyzeData(sensorData);

            };

            channel.BasicConsume(queue: qName,
                                 autoAck: true,
                                 consumer: consumer);
        }


    }
}
