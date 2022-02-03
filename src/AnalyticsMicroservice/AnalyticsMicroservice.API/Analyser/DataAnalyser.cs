using AnalyticsMicroservice.API.Entities;
using AnalyticsMicroservice.API.Repository;
using Microsoft.Extensions.Caching.Distributed;

namespace AnalyticsMicroservice.API.Analyser
{
    public class DataAnalyser
    {
        private IAnalyticsRepository _repository;
        //private IDistributedCache _redis = new ;
        //u receiver konstruktor primas isto IDistributedCache i u konstruktoru ga postavljas, kad pravis DataAnalyser prosledis mu taj objekat
        // a DataAnalyser ce da ga prosledi kroz svoj konstruktor repozitorijumu
        public DataAnalyser(IDistributedCache redis)
        {

            _repository = new AnalyticsRepository(redis);
        }

        public DataAnalytics AnalyzeData(Data data)
        {
            DataAnalytics dA = new DataAnalytics();

            if (data.SensorType == "Pm10")
            {
                if (data.Value < 30)
                    dA.Risk = "green";
                else if (data.Value > 30 && data.Value < 100)
                    dA.Risk = "yellow";
                else
                    dA.Risk = "red";
            }
            else if (data.SensorType == "Pm25")
            {
                if (data.Value < 20)
                    dA.Risk = "green";
                else if (data.Value > 20 && data.Value < 60)
                    dA.Risk = "yellow";
                else
                    dA.Risk = "red";
            }
            else if (data.SensorType == "Ozone")
            {
                if (data.Value < 120)
                    dA.Risk = "green";
                else if (data.Value > 120 && data.Value < 240)
                    dA.Risk = "yellow";
                else
                    dA.Risk = "red";
            }
            else if (data.SensorType == "SO2")
            {
                if (data.Value < 100)
                    dA.Risk = "green";
                else if (data.Value > 100 && data.Value < 500)
                    dA.Risk = "yellow";
                else
                    dA.Risk = "red";
            }

            dA.SensorType = data.SensorType;
            dA.Id = data.Id;
            dA.Value = data.Value;

            return dA;

        }
    }
}
