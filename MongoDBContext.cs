using MongoDB.Driver;

namespace MausWorks.MongoDB
{
    public abstract class MongoDBContext
	{
        internal IMongoClient MongoClient { get; set; }
		internal IMongoDatabase Database { get; set; }

        /// <summary>
        /// Creates the a <see cref="IMongoCollection{TDocument}"/> and maps it to the connected database.
        /// </summary>
        /// <typeparam name="T">The type of the collection</typeparam>
        /// <param name="collectionName">The name of the collection as specified in the database.</param>
        /// <returns></returns>
        public IMongoCollection<T> CreateCollection<T>(string collectionName) where T : class
        {
            return Database.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Used to set up and configure the collections in this <see cref="MongoDBContext"/> 
        /// <para>Called after the context has successfully been created.</para>
        /// </summary>
        public abstract void SetupCollections();
    }
}