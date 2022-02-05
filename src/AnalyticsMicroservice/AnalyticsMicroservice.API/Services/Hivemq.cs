using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnalyticsMicroservice.Services
{
    public class Hivemq
    {
        private IMqttClient _client;

        public Hivemq()
        {
            _client = (new MqttFactory()).CreateMqttClient();
        }

        public async Task Connect()
        {
            try
            {
                await _client.ConnectAsync(
                new MqttClientOptionsBuilder()
                    .WithTcpServer("hivemq", 1883)
                    .WithCleanSession(true)
                    .Build(),
                CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine("MQTT Connect failed: " + e.Message);
            }
        }

        public async Task Discoonnect()
        {
            try
            {
                await _client.DisconnectAsync();
                Console.WriteLine("Disconnected");
            }
            catch (Exception e)
            {
                Console.WriteLine("Connect failed: " + e.Message);
            }
        }

        public bool IsConnected()
        {
            return _client.IsConnected;
        }
        public async Task Subscribe(string topic, Action<MqttApplicationMessageReceivedEventArgs> callback)
        {
            try
            {
                await _client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
                _client.UseApplicationMessageReceivedHandler(callback);
            }
            catch (Exception e)
            {
                Console.WriteLine("Subscribe failed: " + e.Message);
            }
        }
        public async Task Publish(object data, string topic)
        {
            try
            {
                MqttApplicationMessage message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(JsonConvert.SerializeObject(data))
                    .WithRetainFlag()
                    .Build();

                await _client.PublishAsync(message, CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine("Publish failed: " + e.Message);
            }
        }
    }
}