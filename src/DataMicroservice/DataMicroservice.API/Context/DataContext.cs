using DataMicroservice.API.Entities;
using MongoDB.Driver;

namespace DataMicroservice.API.Context
{
    public class DataContext : IDataContext
    {
        public DataContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            AllData = database.GetCollection<Data>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
        }

        public IMongoCollection<Data> AllData { get; }
    }
}
