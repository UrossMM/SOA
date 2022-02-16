using CommandMicroservice.API.Entities;
using CommandMicroservice.API.Hubs;
using CommandMicroservice.Services;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using System.Text;

namespace CommandMicroservice.API.Commander
{
    public class DataCommander
    {
        private Hivemq _mqttService;
        private event EventHandler ServiceCreated;
        private CommandHub _hubContext;

        private bool night= false;

        public void isNight(bool n)
        {
            night = n;
        }

        public DataCommander(Hivemq mqttService, CommandHub hubContext)
        {
            _hubContext = hubContext;
            _mqttService = mqttService;
            ServiceCreated += OnServiceCreated;
            ServiceCreated?.Invoke(this, EventArgs.Empty);
            Console.WriteLine("Napravljen datacommander");

        }

        private async void OnServiceCreated(object sender, EventArgs args)
        {
            Console.WriteLine("OnServiceCreated");
            while (!_mqttService.IsConnected())
            {
                await _mqttService.Connect();
            }
            if (_mqttService.IsConnected())
            {
                Console.WriteLine("Zovem OnDataReceived");

                await _mqttService.Subscribe("sensor/analytics", OnDataReceived);
            }
        }

        private async void OnDataReceived(MqttApplicationMessageReceivedEventArgs arg)
        {
            try
            {
                //Console.WriteLine("Uso u ONDATAReceived");

                var bds = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
                var des = System.Text.Json.JsonSerializer.Deserialize<DataAnalytics>(bds);
                //Console.WriteLine(des.SensorType);
                Console.WriteLine(des.Risk);
                await CommandAsync(des);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public async Task CommandAsync(DataAnalytics receivedObject)
        {
            HttpClient httpClient = new HttpClient();

            /*DateTime dt = DateTime.Now;
            int hour = dt.Hour;
            Console.WriteLine("HOURS: " + hour);
            if (hour < 5)
                night = true;*/

            if (night)
            {
                var responseMessage = await httpClient.PostAsJsonAsync("http://sensordevicemicroservice.api:80/api/SensorDevice/StopSensor", receivedObject.SensorType);
                Console.WriteLine("UGASEN SENZOR");
                Console.WriteLine(responseMessage);

                await _hubContext.SendWarning($"Sensor: {receivedObject.SensorType} is off!");

                return;
            }
            else
            { 
                if (receivedObject.Risk == "green")
                {
                   // Console.WriteLine("GREEN-ZOVE SENSOR");
                    var responseMessage = await httpClient.PostAsJsonAsync("http://sensordevicemicroservice.api:80/api/SensorDevice/SetTimeout/9000", receivedObject.SensorType);
                    Console.WriteLine("POZVAN SENZOR");
                    Console.WriteLine(responseMessage);

                    await _hubContext.SendWarning($"Sensor: {receivedObject.SensorType} has {receivedObject.Risk} zone!");

                    
                }
                else //yellow or red zone
                {
                    var responseMessage = await httpClient.PostAsJsonAsync("http://sensordevicemicroservice.api:80/api/SensorDevice/SetTimeout/4000", receivedObject.SensorType);
                    Console.WriteLine("POZVAN SENZOR");
                    Console.WriteLine(responseMessage);

                    await _hubContext.SendWarning($"Sensor: {receivedObject.SensorType} has {receivedObject.Risk} zone!");
                }
            }
            }
        
    }
}
