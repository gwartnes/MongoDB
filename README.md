# MausWorks.MongoDB Service

A "Entity Framework-esque" service for MongoDB for usage in ASP.NET 5.

#Example usage:

##Startup.cs
    public void ConfigureServices(IServiceCollection services)
    {
		// No service for "MongoDB" has to be added or configured,
		// only configuration for the contexts.
        services
        	.AddMongoDBContext<SomeContext>(Configuration.GetConfigurationSection("MongoDB:SomeContext"));
        
        // alternatively:
        services.AddMongoDBContext<SomeContext>(o =>
		{
			o.ConnectionString = "mongodb://usr:passw@mongoserver.tld:1337/database-name";
			o.DatabaseName = "database-name"; //or null if part of the connection-string
		});

		// Further configuration can be used as such:
        services.ConfigureMongoDBContext<SomeContext>(o =>
        {
            o.ClientSettings = new MongoClientSettings
            {
                UseSsl = true,
                WriteConcern = WriteConcern.WMajority,
                ConnectionMode = ConnectionMode.Standalone
            };
            o.DatabaseSettings = new MongoDatabaseSettings
            {
                ReadPreference = ReadPreference.PrimaryPreferred,
                WriteConcern = new WriteConcern(1)
            };
        });
    }
    
##SomeContext.cs
    public class SomeContext : MongoDBContext
    {
        public IMongoCollection<SomeEntity> SomeEntities { get; set; }

        public override void SetupCollections()
        {
            SomeEntities = CreateCollection<SomeEntity>("SomeEntities");
        }
    }
	
##Example configuration (config.json)
    {
    	"MongoDB": {
    		"SomeContext": {
    			"ConnectionString": "mongodb://usr:passw@mongoserver.tld:1337/database-name",
    			"DatabaseName": "database-name"
    		}
    	}
    }

