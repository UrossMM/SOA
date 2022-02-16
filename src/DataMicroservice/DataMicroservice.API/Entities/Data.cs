using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataMicroservice.API.Entities
{
    public class Data
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string City { get; set; }
        public string Site { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string SensorType { get; set; }
        public decimal Value { get; set; }
        /* public decimal Pm25 { get; set; }
         public decimal Pm10 { get; set; }   
         public decimal SO2 { get; set; }    
         public decimal Ozone { get; set; }  */

    }
}
