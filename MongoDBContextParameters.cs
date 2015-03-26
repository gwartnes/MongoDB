using MausWorks.MongoDB.Annotations;
using System;

namespace MausWorks.MongoDB
{
	public class MongoDBContextParameters
	{
		public string DatabaseName { get; set; }
		public string ConnectionString { get; set; }

		public MongoDBContextParameters([NotNull] string name, [NotNull] string connectionString, [CanBeNull]  string databaseName)
		{
			ConnectionString = connectionString;

			if (databaseName == null)
			{
				var parts = connectionString.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

				if (parts.Length < 3)
				{
					throw new ArgumentNullException("databaseName", String.Format("A database name was not provided for '{0}' and is not part of the connection-string. Please include a database name for '{0}'", name));
				}

				databaseName = parts[parts.Length - 1].Trim();
			}
			DatabaseName = databaseName;
		}

		public MongoDBContextParameters([NotNull] string name, [NotNull] string connectionString) : this(name, connectionString, null)
		{

		}
    }
}