using AnalyticsMicroservice.API.Entities;
using AnalyticsMicroservice.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsMicroservice.API.Controllers
{
        [Route("/api/[controller]/[action]")]
        [ApiController]
        public class AnalyticsController : ControllerBase
        {
            private readonly IAnalyticsRepository _repository;

            public AnalyticsController(IAnalyticsRepository repository)
            {
                _repository = repository;
            }

            [HttpPost]
            public async Task<ActionResult> WriteToMongo([FromBody] DataAnalytics dA)
            {
                await _repository.WriteToMongo(dA);
                return Ok();
            }

            [HttpGet("{risk}")]
            public async Task<ActionResult<DataAnalytics>> GetAnalyticsData(string risk)
            {
                var data = await _repository.GetDataByRisk(risk);
                return Ok(data);
            }

            [HttpDelete]
            public async Task DeleteAllRecords()
            {
            await _repository.DeleteAllRecords();
            }
    }
    
}
