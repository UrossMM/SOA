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
        private const int V = 60;
        private ConnectionFactory factory;
        private IModel channel;
        private IConnection connection;
        private DataAnalyser _da;

        public Receiver()
        {
            _da = new DataAnalyser();

            /*factory = new ConnectionFactory();
            factory.UserName = "user";
            factory.Password = "password";
            factory.VirtualHost = "/";
            //factory.Protocol = Protocols.FromEnvironment();
            factory.HostName = "rabbitmq";
            factory.Port = 5672;
            connection = factory.CreateConnection();*/

            factory = new ConnectionFactory()
            {
                //HostName = "rabbitmq",
                HostName = "192.168.99.100:15672",
                Port = AmqpTcpEndpoint.UseDefaultPort,
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                DispatchConsumersAsync = false
            };
            var endpoints = new System.Collections.Generic.List<AmqpTcpEndpoint> {
                      new AmqpTcpEndpoint("hostname"),
                      new AmqpTcpEndpoint("rabbitmq")
                };
            if (connection == null)
                connection = factory.CreateConnection(endpoints);
            if (channel == null)
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
