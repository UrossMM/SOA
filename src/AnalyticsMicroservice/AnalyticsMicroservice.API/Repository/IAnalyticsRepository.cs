using AnalyticsMicroservice.API.Entities;

namespace AnalyticsMicroservice.API.Repository
{
    public interface IAnalyticsRepository
    {
        Task WriteToMongo(DataAnalytics data);
        Task<IEnumerable<DataAnalytics>> GetDataByRisk(string risk);
        Task<IEnumerable<DataAnalytics>> GetAllData();
        Task DeleteAllRecords();

    }
}
