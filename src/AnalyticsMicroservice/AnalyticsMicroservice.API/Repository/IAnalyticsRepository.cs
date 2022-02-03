using AnalyticsMicroservice.API.Entities;

namespace AnalyticsMicroservice.API.Repository
{
    public interface IAnalyticsRepository
    {
        Task WriteToRedis(DataAnalytics data);
        Task<DataAnalytics> GetAnalyticsData(int id);
    }
}
