using Amazon.Runtime.Documents;
using backend.Entities;
using Backend.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace backend.Repositories
{
    public class DatabaseRepository<T> : IDatabaseRepository<T> where T : Entities.Document
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
    }
}
