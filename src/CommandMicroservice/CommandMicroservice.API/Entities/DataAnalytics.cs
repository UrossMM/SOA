using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CommandMicroservice.API.Entities
{
        public class DataAnalytics
        {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string SensorType { get; set; }
            public decimal Value { get; set; }
            public string Risk { get; set; }

        }

}
