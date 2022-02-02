namespace SensorDeviceMicroservice.API.Model
{
    [Serializable]
    public class Sensor
    {
        public string SensorType { get; set; }
        public double Value { get; set; }

        public Sensor(string type, double value)
        {
            this.SensorType = type;

            this.Value = value;
        }
    }
}
