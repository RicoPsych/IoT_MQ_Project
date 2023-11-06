using backend.Entities;

namespace backend.Repositories
{
    public interface ITemperatureRepository : IDatabaseRepository<Temperature> { }
    

    public class TemperatureRepository : DatabaseRepository<Temperature>, ITemperatureRepository
    {
    public TemperatureRepository(IConfiguration configuration) : base(configuration)
    {
    }
    }
    public class AltitudeRepository : DatabaseRepository<Altitude>
    {
        public AltitudeRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
    public class BatteryRepository : DatabaseRepository<Battery>
    {
        public BatteryRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
    public class DistanceRepository : DatabaseRepository<Distance>
    {
        public DistanceRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
