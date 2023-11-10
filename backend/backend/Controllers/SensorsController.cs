using backend.Entities;
using backend.Repositories;
using Backend.Filters;
using Backend.Models;
using Backend.Services;
using Backend.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorsController : ControllerBase
    {

        private readonly ILogger<SensorsController> _logger;
        private readonly SensorsService _sensorsService;

        public SensorsController(ILogger<SensorsController> logger,
            SensorsService sensorsService)
        {
            _logger = logger;
            _sensorsService = sensorsService;
        }


        [HttpDelete]
        public void Clear()
        {
            _sensorsService.Clear();
            Ok();
            return; 
        }



        [HttpGet("GetAvg")]
        public decimal GetAvg([FromQuery] string[] types, [FromQuery] int[] instances)
        {
            return _sensorsService.GetAverage(types,instances, 100);
        }

        [HttpGet("GetLast")]
        public PresentationModel GetLast([FromQuery] string[] types, [FromQuery] int[] instances)
        {
            return new PresentationModel(_sensorsService.GetLast(types.Select(type=> type.ToLower()).ToArray(), instances));
        }


        [HttpGet("GetAll")]
        public IEnumerable<PresentationModel> GetAll()
        {
            return _sensorsService.GetAll().Select(measurement=>new PresentationModel(measurement));
        }

        [HttpGet("Get")]
        public IEnumerable<PresentationModel> Get(
            [FromQuery] Sort? sort, 
            [FromQuery] Filters? filters )
        {
            return _sensorsService.Get(filters ?? new Filters(), sort ?? new Sort()).Select(measurement => new PresentationModel(measurement));
        }

        [HttpGet("GetCsv")]
        public FileResult GetCsv(
            [FromQuery] Sort? sort,
            [FromQuery] Filters? filters)
        {
            var csvContent = CsvSerializer.SerializeToUtf8Bytes(_sensorsService.Get(filters ?? new Filters(), sort ?? new Sort())
                .Select(measurement => new PresentationModel(measurement)));       
            return File(csvContent, "text/csv", "Measurements.csv");
        }

        [HttpGet("GetJson")]
        public FileResult GetJson(
            [FromQuery] Sort? sort,
            [FromQuery] Filters? filters)
        {
            var jsonContent = JsonSerializer.SerializeToUtf8Bytes(_sensorsService.Get(filters ?? new Filters(), sort ?? new Sort())
                .Select(measurement => new PresentationModel(measurement)));
            return File(jsonContent, "application/json", "Measurements.json");

        }
    }
}