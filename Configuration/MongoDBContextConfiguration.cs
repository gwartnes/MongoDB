using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace MausWorks.MongoDB.Configuration
{
    /// <summary>
    /// Configuration class for <see cref="MongoDBContext"/>s
    /// </summary>
    public class MongoDBContextConfiguration
    {
        /// <summary>
        /// Gets or sets the mongo client settings.
        /// </summary>
        /// <value>
        /// The mongo client settings.
        /// </value>
        public MongoClientSettings ClientSettings { get; set; } = null;

        /// <summary>
        /// Gets or sets the mongo database settings.
        /// </summary>
        /// <value>
        /// The mongo database settings.
        /// </value>
        public MongoDatabaseSettings DatabaseSettings { get; set; } = null;

        private ConnectionString _connectionString;

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString
        {
            get { return _connectionString == null ? null : _connectionString.ToString(); }
            set
            {
                _connectionString = new ConnectionString(value);

                DatabaseName = DatabaseName ?? _connectionString.DatabaseName;
            }
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="MongoDBContextConfiguration"/>.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        internal string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBContextConfiguration"/> class.
        /// </summary>
        public MongoDBContextConfiguration()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBContextConfiguration"/> class.
        /// </summary>
        /// <param name="name">The name of the context.</param>
        public MongoDBContextConfiguration(string name)
        {
            Name = name;
        }
    }

}