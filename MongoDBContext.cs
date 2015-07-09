using MausWorks.MongoDB.Exceptions;
using MongoDB.Driver;
using System;

namespace MausWorks.MongoDB
{
	public abstract class MongoDBContext
	{
        internal IMongoClient MongoClient { get; set; }
		internal IMongoDatabase Database { get; set; }
        
        public IMongoCollection<T> CreateCollection<T>(string collectionName) where T : class
        {
            return Database.GetCollection<T>(collectionName);
        }

        public abstract void SetupCollections();
    }
}