using DataMicroservice.API.Context;
using DataMicroservice.API.Entities;
using MongoDB.Driver;

namespace DataMicroservice.API.Repository
{
    public class AirRepository : IAirRepository
    {
        private readonly IDataContext _dbContext;

        public AirRepository(IDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddData(Data data)
        {
            await _dbContext.AllData.InsertOneAsync(data);
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
