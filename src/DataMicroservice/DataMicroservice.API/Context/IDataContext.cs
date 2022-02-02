using DataMicroservice.API.Entities;
using MongoDB.Driver;

namespace DataMicroservice.API.Context
{
    public interface IDataContext
    {
        IMongoCollection<Data> AllData { get; }
    }
}
