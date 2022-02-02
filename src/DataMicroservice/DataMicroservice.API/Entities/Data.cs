namespace DataMicroservice.API.Entities
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
        public string SensorType { get; set; } //so2(sumpor dioksid), ozone(azot dioksid)
        public decimal Value { get; set; }
        /* public decimal Pm25 { get; set; }
         public decimal Pm10 { get; set; }   
         public decimal SO2 { get; set; }    
         public decimal Ozone { get; set; }  */

    }
}
