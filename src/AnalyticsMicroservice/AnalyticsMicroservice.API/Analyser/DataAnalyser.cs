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

            if (data.SensorType == "Pm10")
            {
                if (data.Value < 30)
                    dA.Risk = "green";
                else if (data.Value > 30 && data.Value < 100)
                    dA.Risk = "yellow";
                else
                    dA.Risk = "red";
            }
            else if (data.SensorType == "Pm25")
            {
                if (data.Value < 20)
                    dA.Risk = "green";
                else if (data.Value > 20 && data.Value < 60)
                    dA.Risk = "yellow";
                else
                    dA.Risk = "red";
            }
            else if (data.SensorType == "Ozone")
            {
                if (data.Value < 120)
                    dA.Risk = "green";
                else if (data.Value > 120 && data.Value < 240)
                    dA.Risk = "yellow";
                else
                    dA.Risk = "red";
            }
            else if (data.SensorType == "SO2")
            {
                if (data.Value < 100)
                    dA.Risk = "green";
                else if (data.Value > 100 && data.Value < 500)
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
