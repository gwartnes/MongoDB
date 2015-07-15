using MongoDB.Driver;

namespace MausWorks.MongoDB
{
    /// <summary>
    /// Provides templating for future <see cref="MongoDBContext"/>s
    /// </summary>
    public abstract class MongoDBContext
    {
        /// <summary>
        /// Gets or sets the mongo client.
        /// </summary>
        /// <value>
        /// The mongo client.
        /// </value>
        internal IMongoClient MongoClient { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
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