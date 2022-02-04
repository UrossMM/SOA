using Microsoft.AspNetCore.Mvc;
using SensorDeviceMicroservice.API.Model;
using SensorDeviceMicroservice.API.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SensorDeviceMicroservice.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SensorDeviceController : ControllerBase
    {
        private readonly SensorServiceList _sensorsList;
        public SensorDeviceController()
        {
            _sensorsList = SensorServiceList.GetSensorsListInstance();
        }

        [HttpPost]
        public IActionResult StartSensor([Required, FromBody] string type)
        {
            foreach (SensorService sensor in _sensorsList.GetSensors())
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    sensor.SensorStart();
                    return Ok($"Sensor {sensor.SensorType} started successfully");
                }
            }

            return BadRequest("SensorType doesn't exist");
        }

        [HttpPost]
        public IActionResult StopSensor([Required, FromBody] string type)
        {
            foreach (SensorService sensor in _sensorsList.GetSensors())
            {
                if (type.ToLower() == sensor.SensorType.ToLower())
                {
                    sensor.SensorStop();
                    return Ok($"Sensor {sensor.SensorType} stop successfully");
                }
            }

            return BadRequest("SensorType doesn't exist");

        }

        [HttpGet]
        public IActionResult GetSensorMetadata([Required] string type)
        {
            if (type == null)
                return BadRequest($"Sensor with specified type does not exist!");

            foreach (SensorService sensor in _sensorsList.GetSensors())
            {
                if (type.ToLower() == sensor.DataToProceed.SensorType.ToLower())
                {
                    SensorMetadata metadata = new SensorMetadata(type, sensor.Timeout.ToString(), sensor.Threshold.ToString());

                    return Ok(metadata);
                }
            }
            return BadRequest($"Sensor type: {type} doesn't exist!");
        }

        [HttpGet]
        public IActionResult GetAllSensorsParams()
        {

            return Ok(_sensorsList.GetSensors());
        }

        [HttpGet]
        public IActionResult GetTimeout([Required] string type)
        {
            foreach (var sensor in _sensorsList.GetSensors())
            {
                if (type.ToLower() == sensor.DataToProceed.SensorType.ToLower())
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };

                    string timeoutInfo = JsonSerializer.Serialize(new
                    {
                        isTimeout = !sensor.IsThresholdSet,
                        value = sensor.Timeout
                    }, options);

                    return Ok(timeoutInfo);
                }
            }
            return BadRequest("Sensor doesn't exist");
        }

        [HttpPost]
        public IActionResult SetTimeout([Required, FromBody] string type, [Required] double? value)
        {
            foreach (var sensor in _sensorsList.GetSensors())
            {
                if (type.ToLower() == sensor.DataToProceed.SensorType.ToLower())
                {
                    sensor.IsThresholdSet = false;
                    if (value != null)
                    {
                        sensor.SetTimeout((double)value);
                      
                        return Ok($"Timeout based measuring started for {type} sensor. New Timeout value set: {value}");
                    }
                    else
                    {
                        return Ok($"Timeout based measuring started for {type} sensor. Last Timeout value used");
                    }
                }
            }
            return BadRequest("Sensor does");
        }

        [HttpGet]
        public IActionResult GetThreshold([Required] string type)
        {
            foreach (var sensor in _sensorsList.GetSensors())
            {
                if (type.ToLower() == sensor.DataToProceed.SensorType.ToLower())
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    };
                    string tresholdInfo = JsonSerializer.Serialize(new { isTreshold = sensor.IsThresholdSet, value = sensor.Threshold }, options);
                    return Ok(tresholdInfo);
                }
            }
            return BadRequest("Sensor doesn't exist");
        }

        [HttpPost]
        public IActionResult SetThreshold([Required, FromBody] string type, [Required] double? value)
        {
            if (value == null) return BadRequest("Threshold value must not be null");

            foreach (var sensor in _sensorsList.GetSensors())
            {
                if (type.ToLower() == sensor.DataToProceed.SensorType.ToLower())
                {
                    sensor.IsThresholdSet = true;
                    if (value != null)
                    {

                        sensor.Threshold = (float)value;

                        return Ok($"Threshold based measuring started for {type} sensor. New Threshold value set {value}");
                    }
                    else
                    {
                        return Ok($"Threshold based measuring started for {type} sensor. Default Threshold value used");
                    }
                }
            }
            return BadRequest("Type of sensor doesn't exist");
        }

    }
}
