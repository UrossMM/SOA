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
            public async Task<ActionResult> WriteToRedis([FromBody] DataAnalytics dA)
            {
                await _repository.WriteToMongo(dA);
                return Ok();
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<DataAnalytics>> GetAnalyticsData(int id)
            {
                var data = await _repository.GetDataById(id);
                return Ok(data);
            }
        }
    
}
