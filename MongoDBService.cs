using Microsoft.Framework.ConfigurationModel;
using System.Collections.Immutable;
using System.Linq;
using System.Collections.Generic;

namespace MausWorks.MongoDB
{
	public class MongoDBService
	{
		public ImmutableDictionary<string, MongoDBContextParameters> Contexts { get; set; }
		
		public MongoDBService(IConfiguration config)
		{
			Contexts = ImmutableDictionary.CreateRange
			(
				config.GetSubKeys("MongoDB")
				.Select
				(c => 
					new KeyValuePair<string, MongoDBContextParameters>
					(
						c.Key, 
						new MongoDBContextParameters(c.Key, c.Value.Get("ConnectionString"), c.Value.Get("DatabaseName"))
					)
				)
			);
		}
	}
}