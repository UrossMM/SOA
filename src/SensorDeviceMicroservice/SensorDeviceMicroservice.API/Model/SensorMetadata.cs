namespace SensorDeviceMicroservice.API.Model
{
    [Serializable]
    public class SensorMetadata
    {
        public string SensorType { get; set; }
        public string Timeout { get; set; }
        public string Treshold { get; set; }

        public SensorMetadata(string type, string timeout, string treshold)
        {
            this.SensorType = type;
            this.Timeout = timeout;
            this.Treshold = treshold;
        }
    }
}
