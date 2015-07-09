using MongoDB.Driver.Core.Configuration;
using MausWorks.MongoDB.Annotations;
using System;

namespace MausWorks.MongoDB.Configuration
{
	public class MongoDBContextConfiguration
	{
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
        /// Gets or sets the name of the MongoDBContext.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

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