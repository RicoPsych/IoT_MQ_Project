using backend.Entities;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDatabaseRepository<Temperature> _temperatureRepository;
        private readonly IDatabaseRepository<Altitude> _altitudeRepository;
        private readonly IDatabaseRepository<Distance> _distanceRepository;
        private readonly IDatabaseRepository<Battery> _batteryRepository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDatabaseRepository<Temperature> temperatureRepository, IDatabaseRepository<Altitude> altitudeRepository, IDatabaseRepository<Distance> distanceRepository, IDatabaseRepository<Battery> batteryRepository)
        {
            _logger = logger;
            _temperatureRepository = temperatureRepository;
            _altitudeRepository = altitudeRepository;
            _distanceRepository = distanceRepository;
            _batteryRepository = batteryRepository;
        }



        [HttpGet(Name = "GetSensors")]
        public IEnumerable<Temperature> GetSensors()
        {
            return _temperatureRepository.GetAll().ToArray();

        }
    }
}