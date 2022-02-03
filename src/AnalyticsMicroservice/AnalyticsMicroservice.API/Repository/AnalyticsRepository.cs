using AnalyticsMicroservice.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

//using ce faliti za rabbit
namespace AnalyticsMicroservice.API.Repository
{
    public class AnalyticsRepository: IAnalyticsRepository
    {
        private readonly IDistributedCache _redisCache;

        public AnalyticsRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<DataAnalytics> GetAnalyticsData(int id)
        {
            var aData = await _redisCache.GetStringAsync(id.ToString());

            if (String.IsNullOrEmpty(aData))
                return null;

            return JsonConvert.DeserializeObject<DataAnalytics>(aData);
        }

        public async Task WriteToRedis(DataAnalytics data)
        {
            await _redisCache.SetStringAsync(data.Id.ToString(), JsonConvert.SerializeObject(data));
        }

        public async Task DeleteData(int id)
        {
            await _redisCache.RemoveAsync(id.ToString());
        }
    }
}
