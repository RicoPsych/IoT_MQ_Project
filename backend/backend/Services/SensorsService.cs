using Amazon.Runtime.Internal.Transform;
using backend.Entities;
using backend.Repositories;
using Backend.Filters;
using Backend.Models;
using Microsoft.OpenApi.Extensions;
using System.Collections.Generic;

namespace Backend.Services
{
    public class SensorsService
    {
        private readonly ILogger<SensorsService> _logger;
        private readonly DatabaseRepository<Measurement> _repository;

        public SensorsService(ILogger<SensorsService> logger,
            DatabaseRepository<Measurement> repository
            )
        {
            _logger = logger;
            _repository = repository;
        }

        public void Clear()
        {
            _repository.Clear();
        }


        public decimal GetAverage(string[] types, int[] instances, int limit)
        {
            var list = _repository.Get(null, new Filters.Filters { SensorTypes = types, Limit = limit, Instances = instances });
            return list.Select(measurement => measurement.Value).DefaultIfEmpty(0).Average();
        }

        public Measurement GetLast(string[] types, int[] instances)
        {
            var list = _repository.Get(null, new Filters.Filters { SensorTypes = types, Limit = 1 , Instances = instances });
            return list.FirstOrDefault(new Measurement());
        }

        public IEnumerable<Measurement> GetAll() {
            return _repository.Get(null);
        }

        //public IEnumerable<Measurement> Get(int? limit,
        //                            int? sortBy,
        //                            IEnumerable<string>? sensorTypes,
        //                            IEnumerable<int>? instances,
        //                            DateTime? startTime,
        //                            DateTime? endTime)
        //{
        //    var list = _repository.Get(limit, SortBy.Time, SortOrder.Ascending, sensorTypes, instances, startTime, endTime);  
        //    return list;
        //}

        public IEnumerable<Measurement> Get(Filters.Filters filters, Sort sort)
        {
            var list = _repository.Get(sort, filters);
            return list;
        }
    }

}
