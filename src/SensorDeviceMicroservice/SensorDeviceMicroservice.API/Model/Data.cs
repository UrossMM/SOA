namespace SensorDeviceMicroservice.API.Model
{
    public class Data
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string SiteName { get; set; }
        public string Site { get; set; }
        public string QueryName { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public string SensorType { get; set; }
        public decimal Value { get; set; }
    }
}
