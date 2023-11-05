using Amazon.Runtime.Documents;
using backend.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace backend.Repositories
{
    public class DatabaseRepository<T> : IDatabaseRepository<T> where T : IDocument
    {
        IConfiguration _configuration;
        IMongoCollection<T> collection;

        public DatabaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            string address = _configuration.GetSection("Database")["Address"];
            string port = _configuration.GetSection("Database")["Port"];
            string name = _configuration.GetSection("Database")["Name"];
            string user = _configuration.GetSection("Database")["User"];
            string password = _configuration.GetSection("Database")["Password"];

            MongoClient dbClient = new MongoClient($"mongodb://{user}:{password}@{address}:{port}");

            var database = dbClient.GetDatabase(name);
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
