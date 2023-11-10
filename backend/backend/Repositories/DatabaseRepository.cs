using Amazon.Runtime.Documents;
using backend.Entities;
using Backend.Configuration;
using Backend.Filters;
using Microsoft.OpenApi.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Reflection;
using System.Reflection.Metadata;

namespace backend.Repositories
{
    public class DatabaseRepository<T> where T : Entities.Measurement//: IDatabaseRepository<T> 
    {
        //IConfiguration _configuration;
        IMongoCollection<T> collection;

        public DatabaseRepository(IConfiguration configuration)
        {
            var dbConfiguration = configuration.GetSection("Database").Get<DatabaseConfig>();
            MongoClient dbClient = new MongoClient($"mongodb://{dbConfiguration.User}:{dbConfiguration.Password}@{dbConfiguration.Address}:{dbConfiguration.Port}");

            var database = dbClient.GetDatabase(dbConfiguration.Name);
            collection = database.GetCollection<T>(GetCollectionName(typeof(T)));
        }

        string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute),
                true)
            .FirstOrDefault())?.CollectionName;
        }

        public void Add(T document)
        {
            collection.InsertOne(document);
            return;
        }

        public void Clear()
        {
            var filter = Builders<T>.Filter.Empty;
            collection.DeleteMany(filter);
            return;
        }


        public T FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<T>.Filter.Eq(document => document.Id, objectId);
            return collection.Find(filter).SingleOrDefault();
        }


        public IEnumerable<T> GetAll()
        {
            var filter = Builders<T>.Filter.Empty;
            return collection.Find(filter).ToList();
        }

        public IEnumerable<T> GetFiltered(
            int? limit,
            IEnumerable<string>? sensorTypes = null,
            IEnumerable<int>? instances = null,
            DateTime? startTime = null,
            DateTime? endTime = null)
        {
            limit ??= 100;

            var sensorFilter = ((sensorTypes?.Count() ?? 0) > 0) ? Builders<T>.Filter.In(document => document.SensorType, sensorTypes) : Builders<T>.Filter.Empty;
            var instanceFilter = ((instances?.Count() ?? 0) > 0) ? Builders<T>.Filter.In(document => document.Instance, instances) : Builders<T>.Filter.Empty;
            var dateFilter = startTime is not null ? Builders<T>.Filter.Gte(document => document.Time, startTime) : Builders<T>.Filter.Empty;
            var dateFilter2 = endTime is not null ? Builders<T>.Filter.Lte(document => document.Time, endTime) : Builders<T>.Filter.Empty;

            var finalFilter = sensorFilter & instanceFilter & dateFilter & dateFilter2;

            return collection.Find(finalFilter).Limit(limit).ToList();
        }

        public IEnumerable<T> Get(
            Sort? sortConfig = null,
            Filters? filters = null)           
        {
            sortConfig ??= new Sort();
            filters ??= new Filters();


            IEnumerable<string>? sensorTypes = filters.SensorTypes;
            IEnumerable<int>? instances = filters.Instances;
            DateTime? startTime = filters.StartTime;
            DateTime? endTime = filters.EndTime;
            int? limit = filters.Limit;

            var sensorFilter = ((sensorTypes?.Count() ?? 0) > 0) ? Builders<T>.Filter.In(document => document.SensorType, sensorTypes) : Builders<T>.Filter.Empty;
            var instanceFilter = ((instances?.Count() ?? 0) > 0) ? Builders<T>.Filter.In(document => document.Instance, instances) : Builders<T>.Filter.Empty; 
            var dateFilter = startTime is not null ? Builders<T>.Filter.Gte(document => document.Time, startTime) : Builders<T>.Filter.Empty;
            var dateFilter2 = endTime is not null ? Builders<T>.Filter.Lte(document => document.Time, endTime) : Builders<T>.Filter.Empty;

            var finalFilter = sensorFilter & instanceFilter & dateFilter & dateFilter2;

            
            SortDefinition<T> finalSort;
            if (sortConfig.Order is SortOrder.Ascending)
            {
                finalSort = Builders<T>.Sort.Ascending(sortConfig.By.GetDisplayName());
            }
            else
            {
                finalSort = Builders<T>.Sort.Descending(sortConfig.By.GetDisplayName());
            }

            return collection.Find(finalFilter).Sort(finalSort).Limit(limit).ToList();
        }
    }
}
