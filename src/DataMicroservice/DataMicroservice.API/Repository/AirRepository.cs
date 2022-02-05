using DataMicroservice.API.Context;
using DataMicroservice.API.Entities;
using DataMicroservice.API.Services;
using MongoDB.Driver;

namespace DataMicroservice.API.Repository
{
    public class AirRepository : IAirRepository
    {
        private readonly IDataContext _dbContext;
        private readonly DataService _service;

        public AirRepository(IDataContext dbContext, DataService serv)
        {
            _dbContext = dbContext;
            _service = serv;
        }

        public async Task AddData(Data data)
        {
            await _dbContext.AllData.InsertOneAsync(data);
            _service.PublishOnTopic(data, "sensor/data");
        }

        public async Task<IEnumerable<Data>> GetAllData()
        {
            return await _dbContext
                        .AllData
                        .Find(p => true)
                        .ToListAsync();
        }

        public async Task RemoveAllData()
        {
            await _dbContext.AllData.DeleteManyAsync(p => true);
        }

        public async Task<IEnumerable<Data>> GetDataBySiteName(string siteName)
        {
            return await _dbContext
                        .AllData
                        .Find(x => x.Site == siteName)
                        .ToListAsync();
        }
    }
}
