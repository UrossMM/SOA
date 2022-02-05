using AnalyticsMicroservice.API.Entities;
using AnalyticsMicroservice.API.Repository;
using AnalyticsMicroservice.Services;
using MongoDB.Driver;
using MQTTnet;
using System.Text;

namespace AnalyticsMicroservice.API.Analyser
{
    public class DataAnalyser
    {
        private IAnalyticsRepository _repository;
        private IMongoClient mongo;
        private Hivemq _mqttService;
        private event EventHandler ServiceCreated;

        public DataAnalyser(Hivemq mqttService)
        {
            mongo = new MongoClient("mongodb://analyticsmongo:27017");
            _repository = new AnalyticsRepository(mongo);
            _mqttService = mqttService;
            ServiceCreated += OnServiceCreated;
            ServiceCreated?.Invoke(this, EventArgs.Empty);

        }
        private async void OnServiceCreated(object sender, EventArgs args)
        {
            Console.WriteLine("ONSERVICECREATTED");
            while (!_mqttService.IsConnected())
            {
                await _mqttService.Connect();
            }
            if (_mqttService.IsConnected())
            {
                Console.WriteLine("Zovem ONDATAReceived");

                await _mqttService.Subscribe("sensor/data", OnDataReceived);
            }
        }
        private async void OnDataReceived(MqttApplicationMessageReceivedEventArgs arg)
        {
            try
            {
                Console.WriteLine("Uso u ONDATAReceived");

                var bds = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
                var des = System.Text.Json.JsonSerializer.Deserialize<Data>(bds);
                Console.WriteLine(des.SensorType);

                await AnalyzeData(des);
                //HttpClient httpClient = new HttpClient();
                //var responseMessage = await httpClient.PostAsJsonAsync<SensorTimestamp>("http://192.168.100.22:8006/AnalyticsMicroservice", des);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async void PublishOnTopic(object data, string topic)
        {
            if (_mqttService.IsConnected())
            {
                await _mqttService.Publish(data, topic);
            }
        }
        public async Task AnalyzeData(Data data)
        {
            DataAnalytics dA = new DataAnalytics();

            if (data.SensorType.ToLower() == "Pm10".ToLower())
            {
                if (data.Value < 500)
                    dA.Risk = "green";
                else if (data.Value > 500 && data.Value < 600)
                    dA.Risk = "yellow";
                else
                    dA.Risk = "red";
            }
            else if (data.SensorType.ToLower() == "Pm25".ToLower())
            {
                if (data.Value < 200)
                    dA.Risk = "green";
                else if (data.Value > 200 && data.Value < 300)
                    dA.Risk = "yellow";
                else
                    dA.Risk = "red";
            }
            else if (data.SensorType.ToLower() == "Ozone".ToLower())
            {
                if (data.Value < 20)
                    dA.Risk = "green";
                else if (data.Value > 20 && data.Value < 35)
                    dA.Risk = "yellow";
                else
                    dA.Risk = "red";
            }
            else if (data.SensorType.ToLower() == "SO2".ToLower())
            {
                if (data.Value < 20)
                    dA.Risk = "green";
                else if (data.Value > 20 && data.Value < 33)
                    dA.Risk = "yellow";
                else
                    dA.Risk = "red";
            }

            dA.SensorType = data.SensorType;
            dA.Id = data.Id;
            dA.Value = data.Value;
            Console.WriteLine("Analiza obavljena");

            PublishOnTopic(dA, "sensor/analytics");
            await _repository.WriteToMongo(dA);

        }
    }
}
