namespace AnalyticsMicroservice.API.Entities
{
    public class DataAnalytics
    {
        public int Id { get; set; }
        public string SensorType { get; set; }
        public decimal Value { get; set; }
        public string Risk { get; set; }

    }
}
