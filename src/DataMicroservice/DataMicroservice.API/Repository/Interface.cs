using DataMicroservice.API.Entities;

namespace DataMicroservice.API.Repository
{
    public interface IAirRepository
    {
        Task AddData(Data data);
        Task RemoveAllData();
        Task<IEnumerable<Data>> GetAllData();
        Task<IEnumerable<Data>> GetDataBySiteName(string siteName);

        // dodati jos neke funkcije ako zatreba
    }
}
