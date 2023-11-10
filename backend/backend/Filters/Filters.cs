using Microsoft.AspNetCore.Mvc;

namespace Backend.Filters
{
    public class Filters
    {
        public int? Limit {get;set;} = null;
        public IEnumerable<string>? SensorTypes { get; set; } = null;
        public IEnumerable<int>? Instances { get; set; } = null;
        public DateTime? StartTime { get; set; } = null;
        public DateTime? EndTime { get; set; } = null;
    }

    public class Sort
    {
        public SortBy By { get; set; } = SortBy.Time;
        public SortOrder Order { get; set; } = SortOrder.Descending;
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public enum SortBy
    {
        Time,
        Type,
        Instance,
        Value
    }


}
