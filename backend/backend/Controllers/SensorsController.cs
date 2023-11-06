using backend.Entities;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorsController : ControllerBase
    {

        private readonly ILogger<SensorsController> _logger;
        private readonly IDatabaseRepository<Temperature> _temperatureRepository;
        private readonly IDatabaseRepository<Altitude> _altitudeRepository;
        private readonly IDatabaseRepository<Distance> _distanceRepository;
        private readonly IDatabaseRepository<Battery> _batteryRepository;

        public SensorsController(ILogger<SensorsController> logger,
            IDatabaseRepository<Temperature> temperatureRepository, 
            IDatabaseRepository<Distance> distanceRepository, 
            IDatabaseRepository<Altitude> altitudeRepository, 
            IDatabaseRepository<Battery> batteryRepository)
        {
            _logger = logger;
            _temperatureRepository = temperatureRepository;
            _altitudeRepository = altitudeRepository;
            _distanceRepository = distanceRepository;
            _batteryRepository = batteryRepository;
        }



        [HttpGet("GetTemperature")]
        public IEnumerable<Temperature> GetTemperature()
        {
            return _temperatureRepository.GetAll().ToArray();

        }
        [HttpGet("GetAltitude")]
        public IEnumerable<Altitude> GetAltitude()
        {
            return _altitudeRepository.GetAll().ToArray();

        }
        [HttpGet("GetDistance")]
        public IEnumerable<Distance> GetDistance()
        {
            return _distanceRepository.GetAll().ToArray();

        }
        [HttpGet("GetBattery")]
        public IEnumerable<Battery> GetBattery()
        {
            return _batteryRepository.GetAll().ToArray();

        }
    }
}