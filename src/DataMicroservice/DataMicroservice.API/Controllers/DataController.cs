﻿using DataMicroservice.API.Repository;
using Microsoft.AspNetCore.Mvc;
using DataMicroservice.API.Entities;
//dodati neke usinge
namespace DataMicroservice.API.Controllers
{

    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IAirRepository _repository;
        //private readonly Mqtt _mqtt;


        public DataController(IAirRepository repository)
        {
            //_mqtt = new Mqtt();
            _repository = repository;
            //HttpClient httpClient = new HttpClient();
            //httpClient.PostAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Data>>> GetAllData()
        {
            var data = await _repository.GetAllData();
            return Ok(data);
        }

        [HttpGet("{siteName}")]
        public async Task<ActionResult<IEnumerable<Data>>> GetDataBySiteName(string siteName)
        {
            var data = await _repository.GetDataBySiteName(siteName);
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> AddData([FromBody] Data data)
        {
            if (data == null)
                return BadRequest();

            await _repository.AddData(data);

            //_mqtt.Publish(data, "sensor/data");

            return Ok();
        }

        [HttpDelete]
        public async Task RemoveAllData()
        {
            await _repository.RemoveAllData();
        }
    }
}